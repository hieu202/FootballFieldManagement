using FootballFieldManagement.Core.Commands;
using FootballFieldManagement.Core.Repositories;
using FootballFieldManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FootballFieldManagement.UI.ViewModels
{
    public class FieldBookManagementViewModel : BaseViewModel
    {
        private ObservableCollection<Customer> _listCustomer;
        public ObservableCollection<Customer> ListCustomer { get { return _listCustomer; } set { _listCustomer = value; OnPropertyChanged(); } }
        private ObservableCollection<Field> _listField;
        public ObservableCollection<Field> ListField { get { return _listField; } set { _listField = value; OnPropertyChanged(); } }
        private Customer _selectedCustomer;

        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set { _selectedCustomer = value; OnPropertyChanged(); }
        }
        private Field _selectedField;

        public Field SelectedField
        {
            get { return _selectedField; }
            set { _selectedField = value; OnPropertyChanged(); }
        }
        private DateTime _dateApply = DateTime.Now;
        public DateTime DateApply
        {
            get { return _dateApply; }
            set { _dateApply = value; OnPropertyChanged(); }
        }
        private string _startTime;

        public string StartTime
        {
            get { return _startTime; }
            set { _startTime = value; Validate(); OnPropertyChanged(); }
        }
        private string _endTime;

        public string EndTime
        {
            get { return _endTime; }
            set { _endTime = value; OnPropertyChanged(); Validate(); }
        }
        public string _errorValidateStartTime;
        public string ErrorValidateStartTime
        {
            get { return _errorValidateStartTime; }
            set { _errorValidateStartTime = value; OnPropertyChanged(); }
        }
        public string _errorValidateEndTime;
        public string ErrorValidateEndTime
        {
            get { return _errorValidateEndTime; }
            set { _errorValidateEndTime = value; OnPropertyChanged(); }
        }
        private string _note;

        public string Note
        {
            get { return _note; }
            set { _note = value; OnPropertyChanged(); }
        }

        public ICommand AddCommand { get; set; }
        public FieldBookManagementViewModel()
        {
            Validate();
            LoadCombox();
            AddCommand = new RelayCommand<object>(p =>
            {
                if (ErrorValidateEndTime != "" || ErrorValidateStartTime != "")
                    return false;
                if (String.IsNullOrEmpty(Note) == true || String.IsNullOrEmpty(StartTime) == true || String.IsNullOrEmpty(EndTime) == true)
                    return false;
                return true;
            }, async p =>
            {
                try
                {
                    var pricesQuery = _bookRepository.AsQueryable().Where(x => x.DateApply.Date == DateApply.Date && x.FieldId == SelectedField.Id).ToList();
                    if (pricesQuery.Any() && (StaticClass.ConvertTimeToDecimal(StartTime) < pricesQuery.Max(x => StaticClass.ConvertTimeToDecimal(x.EndTime))
                    || StaticClass.ConvertTimeToDecimal(EndTime) < pricesQuery.Min(x => StaticClass.ConvertTimeToDecimal(x.StartTime))))
                    {
                        MessageBox.Show("Trùng giờ");
                    }
                    else
                    {
                        var fieldBookManagement = new FieldBookManagement()
                        {
                            CustomerId = SelectedCustomer.Id,
                            FieldId = SelectedField.Id,
                            DateApply = DateApply,
                            StartTime = StartTime,
                            EndTime = EndTime,
                            Note = Note,
                            IsDeleted = false
                        };
                        fieldBookManagement = await _bookRepository.AddAsync(fieldBookManagement);
                        if (fieldBookManagement != null)
                        {
                            MessageBox.Show("Thêm lịch đặt sân thành công");
                        }
                        else
                        {
                            MessageBox.Show("Lỗi hệ thống");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            });

        }
        public IRepository<Customer> _customerRepository = new Repository<Customer>(StaticClass.FootballFieldManagementDbContext);
        public IRepository<Field> _fieldRepository = new Repository<Field>(StaticClass.FootballFieldManagementDbContext);
        public IRepository<FieldBookManagement> _bookRepository = new Repository<FieldBookManagement>(StaticClass.FootballFieldManagementDbContext);
        public void LoadCombox()
        {
            _listCustomer = new ObservableCollection<Customer>(_customerRepository.AsQueryable().ToList());
            _listField = new ObservableCollection<Field>(_fieldRepository.AsQueryable().ToList());
            SelectedCustomer = _customerRepository.AsQueryable().FirstOrDefault();
            SelectedField = _fieldRepository.AsQueryable().FirstOrDefault();
        }
        public void Validate()
        {
            if (StartTime != null && IsValidTime(StartTime) == false)
            {
                ErrorValidateStartTime = "Sai định dạng (00:00)";
            }
            if (EndTime != null && IsValidTime(EndTime) == false)
            {
                ErrorValidateEndTime = "Sai định dạng (00:00)";
            }
            if (StartTime == null || IsValidTime(StartTime) == true)
            {
                ErrorValidateStartTime = "";
            }
            if (EndTime == null || IsValidTime(EndTime) == true)
            {
                ErrorValidateEndTime = "";
            }
            if (StartTime != null && IsValidTime(StartTime) == true && EndTime != null && IsValidTime(EndTime) == true)
            {
                if ((StaticClass.ConvertTimeToDecimal(EndTime) - StaticClass.ConvertTimeToDecimal(StartTime)) <= 0.5)
                {
                    ErrorValidateStartTime = "Thời gian ko hợp lệ";
                    ErrorValidateEndTime = "Thời gian ko hợp lệ";
                }
            }
        }
        public static bool IsValidTime(string input)
        {
            Regex regex = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
            return regex.IsMatch(input);
        }
    }
}

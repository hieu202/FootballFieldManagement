using FootballFieldManagement.Core.Commands;
using FootballFieldManagement.Core.Repositories;
using FootballFieldManagement.Domain.Models;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace FootballFieldManagement.UI.ViewModels
{
    public class DisplayFieldBookViewModel : BaseViewModel
    {
        private ObservableCollection<FieldBookManagement> _listFieldBook;

        public ObservableCollection<FieldBookManagement> ListFieldBook
        {
            get { return _listFieldBook; }
            set { _listFieldBook = value; OnPropertyChanged(); }
        }
        private ObservableCollection<Customer> _listCustomer;

        public ObservableCollection<Customer> ListCustomer
        {
            get { return _listCustomer; }
            set { _listCustomer = value; OnPropertyChanged(); }
        }
        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                if (_selectedCustomer != value) // Chỉ thay đổi nếu khác
                {
                    _selectedCustomer = value;
                    LoadData();
                    OnPropertyChanged(nameof(SelectedCustomer));
                }
            }
        }
        private ObservableCollection<Field> _listField;

        public ObservableCollection<Field> ListField
        {
            get { return _listField; }
            set { _listField = value; OnPropertyChanged(); }
        }
        private Field _selectedField;
        public Field SelectedField
        {
            get { return _selectedField; }
            set
            {
                if (_selectedField != value) // Chỉ thay đổi nếu khác
                {
                    _selectedField = value;
                    LoadData();
                    OnPropertyChanged(nameof(SelectedField));
                }
            }
        }
        private ICollectionView _fieldBookCollectionView;

        public ICollectionView FieldBookCollectionView
        {
            get { return _fieldBookCollectionView; }
            set { _fieldBookCollectionView = value; OnPropertyChanged(); }
        }
        private DateTime _startDate = new DateTime(DateTime.Now.Year, 1, 1);

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; OnPropertyChanged(); }
        }
        private DateTime _endDate = new DateTime(DateTime.Now.Year, 12, 31);

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; OnPropertyChanged(); }
        }
        private FieldBookManagement _selectedBookField;

        public FieldBookManagement SelectedBookField
        {
            get { return _selectedBookField; }
            set { _selectedBookField = value; }
        }

        public ICommand DeleteCommand { get; set; }
        public DisplayFieldBookViewModel()
        {
            LoadCombobox();
            LoadData();
            DeleteCommand = new RelayCommand<object>(p =>
            {
                if (SelectedBookField == null)
                    return false;
                return true;
            }, async p =>
            {
                var delete = _fieldBookRepository.AsQueryable().FirstOrDefault(x => x.Id == SelectedBookField.Id);
                delete.IsDeleted = true;
                try
                {
                    delete = await _fieldBookRepository.UpdateAsync(delete);
                    if (delete != null)
                    {
                        MessageBox.Show("Hủy sân thành công");
                    }
                    else
                    {
                        MessageBox.Show("Lỗi hệ thống");
                    }
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }
        public IRepository<FieldBookManagement> _fieldBookRepository = new Repository<FieldBookManagement>(StaticClass.FootballFieldManagementDbContext);
        public IRepository<Customer> _customerRepository = new Repository<Customer>(StaticClass.FootballFieldManagementDbContext);
        public IRepository<Field> _fieldRepository = new Repository<Field>(StaticClass.FootballFieldManagementDbContext);
        private void LoadCombobox()
        {
            ListCustomer = new ObservableCollection<Customer>(_customerRepository.AsQueryable().ToList());
            ListField = new ObservableCollection<Field>(_fieldRepository.AsQueryable().ToList());
        }
        private void LoadData()
        {
            // Khởi tạo truy vấn với Include các thực thể liên quan
            var query = _fieldBookRepository.AsQueryable()
                .Where(x => x.IsDeleted == false)
                .Include(x => x.Customer)
                .Include(x => x.Field).OrderByDescending(x => x.DateApply) as IQueryable<FieldBookManagement>; // Đảm bảo kiểu dữ liệu trả về phù hợp

            // Xây dựng truy vấn sử dụng điều kiện
            query = query.Where(x =>
                (SelectedCustomer == null || SelectedCustomer.Id == -1 || x.Customer.Name == SelectedCustomer.Name) &&
                (SelectedField == null || SelectedField.Id == -1 || x.Field.Name == SelectedField.Name) &&
                (StartDate == null || x.DateApply.Date >= StartDate.Date) &&
                (EndDate == null || x.DateApply.Date <= EndDate.Date));

            // Tạo ObservableCollection từ kết quả truy vấn
            ListFieldBook = new ObservableCollection<FieldBookManagement>(query.ToList());
            FieldBookCollectionView = CollectionViewSource.GetDefaultView(ListFieldBook);
            FieldBookCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Field.Name"));

            if (!ListCustomer.Any(c => c.Id == -1 && c.Name == "Tất cả"))
            {
                ListCustomer.Insert(0, new Customer { Id = -1, Name = "Tất cả" });
                OnPropertyChanged(nameof(ListCustomer));
            }
            //SelectedCustomer = ListCustomer[0];
            if (!ListField.Any(c => c.Id == -1 && c.Name == "Tất cả"))
            {
                ListField.Insert(0, new Field { Id = -1, Name = "Tất cả" });
                OnPropertyChanged(nameof(ListField));
            }
            /*            SelectedField = ListField.FirstOrDefault();
                        SelectedCustomer = ListCustomer.FirstOrDefault();*/

        }
    }
}

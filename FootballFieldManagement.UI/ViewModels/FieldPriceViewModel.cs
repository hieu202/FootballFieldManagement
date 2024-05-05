using FootballFieldManagement.Core.Commands;
using FootballFieldManagement.Core.Repositories;
using FootballFieldManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FootballFieldManagement.UI.ViewModels
{
    public class FieldPriceViewModel : BaseViewModel
    {
        public ObservableCollection<FieldType> _listFieldType;
        public ObservableCollection<FieldType> ListFieldType { get => _listFieldType; set { _listFieldType = value; OnPropertyChanged(); } }
        public ObservableCollection<FieldPrice> _listFieldPrice;
        public ObservableCollection<FieldPrice> ListFieldPrice { get => _listFieldPrice; set { _listFieldPrice = value; OnPropertyChanged(); } }
        private FieldType _selectedFieldType;
        public FieldType SelectedFieldType
        {
            get
            {
                return _selectedFieldType;
            }
            set
            {
                _selectedFieldType = value;
                OnPropertyChanged();
            }
        }
        private string _price;

        public string Price
        {
            get { return _price; }
            set { _price = value; }
        }
        private string _startTime;

        public string StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }
        private string _endTime;

        public string EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }


        public ICommand AddCommand { get; set; }
        private IRepository<FieldType> _fieldTypeRepository = new Repository<FieldType>(StaticClass.FootballFieldManagementDbContext);
        private IRepository<FieldPrice> _fieldPriceRepository = new Repository<FieldPrice>(StaticClass.FootballFieldManagementDbContext);
        public FieldPriceViewModel()
        {
            LoadData();
            LoadComboBox();
            AddCommand = new RelayCommand<object>(p =>
            {
                if (String.IsNullOrEmpty(StartTime) || String.IsNullOrEmpty(EndTime) || String.IsNullOrEmpty(Price))
                    return false;
                return true;
            }, async p =>
            {
                try
                {
                    /*                    var pricesQuery = _fieldPriceRepository.AsQueryable().Where(x => x.FieldTypeId == SelectedFieldType.Id).ToList();
                                        if (pricesQuery.Any() && (StaticClass.ConvertTimeToDecimal(StartTime) < pricesQuery.Max(x => StaticClass.ConvertTimeToDecimal(x.EndTime))
                                        || StaticClass.ConvertTimeToDecimal(EndTime) < pricesQuery.Min(x => StaticClass.ConvertTimeToDecimal(x.StartTime))))
                                        {
                                            MessageBox.Show("Trùng giờ");
                                        }
                                        else
                                        {*/
                    var newFieldPrice = new FieldPrice()
                    {
                        FieldTypeId = SelectedFieldType.Id,
                        /*         StartTime = StartTime,
                                 EndTime = EndTime,*/
                        Price = Double.Parse(Price)
                    };
                    newFieldPrice = await _fieldPriceRepository.AddAsync(newFieldPrice);
                    if (newFieldPrice != null)
                    {
                        MessageBox.Show("Thêm giá sân thành công");
                        LoadData();

                    }
                    else
                    {
                        MessageBox.Show("Lỗi hệ thống");
                    }
                }


                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }
        private void LoadComboBox()
        {
            ListFieldType = new ObservableCollection<FieldType>(_fieldTypeRepository.AsQueryable().ToList());
            SelectedFieldType = ListFieldType.FirstOrDefault();
        }
        private void LoadData()
        {
            ListFieldPrice = new ObservableCollection<FieldPrice>(_fieldPriceRepository.AsQueryable().ToList());
        }
    }
}

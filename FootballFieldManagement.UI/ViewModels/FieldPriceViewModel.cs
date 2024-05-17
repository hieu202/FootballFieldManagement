using FootballFieldManagement.Core.Commands;
using FootballFieldManagement.Core.Repositories;
using FootballFieldManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private FieldPrice _selectedFieldPrice;
        public FieldPrice SelectedFieldPrice
        {
            get
            {
                return _selectedFieldPrice;
            }
            set
            {
                _selectedFieldPrice = value;
                OnPropertyChanged();
                if (SelectedFieldPrice != null)
                {
                    Price = SelectedFieldPrice.Price.ToString();
                    SelectedFieldType = SelectedFieldPrice.FieldType;
                }
            }
        }
        private string _price;

        public string Price
        {
            get { return _price; }
            set { _price = value; OnPropertyChanged(); }
        }
        private string _startTime;
        public ICommand AddCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        private IRepository<FieldType> _fieldTypeRepository = new Repository<FieldType>(StaticClass.FootballFieldManagementDbContext);
        private IRepository<FieldPrice> _fieldPriceRepository = new Repository<FieldPrice>(StaticClass.FootballFieldManagementDbContext);
        public FieldPriceViewModel()
        {
            LoadData();
            LoadComboBox();
            AddCommand = new RelayCommand<object>(p =>
            {
                if (String.IsNullOrEmpty(Price))
                    return false;
                if(_fieldPriceRepository.AsQueryable().Any(x => x.FieldType == SelectedFieldType))
                    return false;
                return true;
            }, async p =>
            {
                try
                {
                    var newFieldPrice = new FieldPrice()
                    {
                        FieldTypeId = SelectedFieldType.Id,

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
            UpdateCommand = new RelayCommand<object>(p =>
            {
                if (SelectedFieldPrice != null && SelectedFieldPrice.FieldType != SelectedFieldType)
                    return false;
                if (String.IsNullOrEmpty(Price))
                    return false;
                return true;
            }, async p =>
            {
                var updateFieldPrice = _fieldPriceRepository.AsQueryable().FirstOrDefault(x => x.Id == SelectedFieldPrice.Id);
                updateFieldPrice.Price = Double.Parse(Price);
                try
                {
                    updateFieldPrice = await _fieldPriceRepository.UpdateAsync(updateFieldPrice);
                    if (updateFieldPrice != null)
                    {
                        MessageBox.Show("Sửa giá sân thành công");
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

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedFieldPrice == null)
                    return false;
                return true;
            }, async p =>
            {
                await _fieldPriceRepository.DeleteAsync(SelectedFieldPrice);
                MessageBox.Show("Xóa giá sân thành công");
                LoadData();
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

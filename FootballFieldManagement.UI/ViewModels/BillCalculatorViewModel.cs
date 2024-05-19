using FootballFieldManagement.Core.Commands;
using FootballFieldManagement.Core.Repositories;
using FootballFieldManagement.Domain.Models;
using FootballFieldManagement.UI.Commands;
using FootballFieldManagement.UI.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace FootballFieldManagement.UI.ViewModels
{
    public class BillCalculatorViewModel : BaseViewModel
    {
        public class Group : BaseViewModel
        {
            private ObservableCollection<ImageItem> _listImage;
            public ObservableCollection<ImageItem> ListImage
            {
                get => _listImage;
                set { _listImage = value; OnPropertyChanged(); }
            }
            public string FieldType { get; set; }
        }
        public class ImageItem : BaseViewModel
        {
            private string _imagePath;
            public string ImagePath
            {
                get => _imagePath;
                set { _imagePath = value; OnPropertyChanged(); }
            }
            private string _contentImage;
            public string ContentImage { get; set; }
            public ICommand FieldCommand { get; set; }
        }

        private string _fieldName;

        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Field> _listField;

        public ObservableCollection<Field> ListField
        {
            get { return _listField; }
            set { _listField = value; OnPropertyChanged(); }
        }
        private ObservableCollection<FieldType> _listFieldType;

        public ObservableCollection<FieldType> ListFieldType
        {
            get { return _listFieldType; }
            set { _listFieldType = value; OnPropertyChanged(); }
        }
        private ObservableCollection<Product> _listProduct = new ObservableCollection<Product>();
        public ObservableCollection<Product> ListProduct
        {
            get { return _listProduct; }
            set { _listProduct = value; OnPropertyChanged(); if (ListProduct != null) CalculatorProduct(); }
        }
        private ObservableCollection<Customer> _listCustomer;

        public ObservableCollection<Customer> ListCustomer
        {
            get { return _listCustomer; }
            set { _listCustomer = value; OnPropertyChanged(); }
        }
        private ObservableCollection<ProductBill> _listProductBill;

        public ObservableCollection<ProductBill> ListProductBill
        {
            get { return _listProductBill; }
            set { _listProductBill = value; OnPropertyChanged(); }
        }
        private ObservableCollection<Group> _listGroup;

        public ObservableCollection<Group> ListGroup
        {
            get { return _listGroup; }
            set { _listGroup = value; OnPropertyChanged(); }
        }
        private ObservableCollection<FieldPrice> _listFieldPrice;

        public ObservableCollection<FieldPrice> ListFieldPrice
        {
            get { return _listFieldPrice; }
            set { _listFieldPrice = value; OnPropertyChanged(); }
        }
        private FieldType _selectedFieldType;
        public FieldType SelectedFieldType
        {
            get { return _selectedFieldType; }
            set { _selectedFieldType = value; OnPropertyChanged(); Load(); }
        }
        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set { _selectedCustomer = value; OnPropertyChanged(); }
        }
        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                if (SelectedProduct != null)
                {
                    Quality = SelectedProduct.Quality.ToString();
                }
                OnPropertyChanged();
            }
        }
        private string _quality;

        public string Quality
        {
            get { return _quality; }
            set { _quality = value; OnPropertyChanged(); }
        }
        private string _fieldPrice;

        public string FieldPrice
        {
            get { return _fieldPrice; }
            set { _fieldPrice = value; OnPropertyChanged(); if (FieldPrice != null) CalculatorTotal(); }
        }
        private string _productPrice = "0";

        public string ProductPrice
        {
            get { return _productPrice; }
            set { _productPrice = value; OnPropertyChanged(); if (ProductPrice != null) CalculatorTotal(); }
        }
        private string _total;

        public string Total
        {
            get { return _total; }
            set { _total = value; OnPropertyChanged(); }
        }
        private string _startTime;

        public string StartTime
        {
            get { return _startTime; }
            set { _startTime = value; OnPropertyChanged(); if (StartTime != null) CalculatorPlayTime(); }
        }
        private string _endTime;

        public string EndTime
        {
            get { return _endTime; }
            set { _endTime = value; OnPropertyChanged(); if (EndTime != null) CalculatorPlayTime(); }
        }
        private string _playTime;

        public string PlayTime
        {
            get { return _playTime; }
            set { _playTime = value; OnPropertyChanged(); }
        }
        private DateTime _datePlay = DateTime.Now;
        public DateTime DatePlay
        {
            get { return _datePlay; }
            set { _datePlay = value; OnPropertyChanged(); }
        }
        private string _validateStartTime;

        public string ValidateStartTime
        {
            get { return _validateStartTime; }
            set { _validateStartTime = value; OnPropertyChanged(); }
        }
        private string _validateEndTime;

        public string ValidateEndTime
        {
            get { return _validateEndTime; }
            set { _validateEndTime = value; OnPropertyChanged(); }
        }


        public ICommand StartCommand { get; set; }
        public ICommand SelectedBookFieldCommand { get; set; }
        IRepository<Field> _fieldRepository = new Repository<Field>(StaticClass.FootballFieldManagementDbContext);
        IRepository<FieldType> _fieldTypeRepository = new Repository<FieldType>(StaticClass.FootballFieldManagementDbContext);
        IRepository<Customer> _customerRepository = new Repository<Customer>(StaticClass.FootballFieldManagementDbContext);
        IRepository<FieldPrice> _fieldPriceRepository = new Repository<FieldPrice>(StaticClass.FootballFieldManagementDbContext);
        IRepository<Bill> _billRepository = new Repository<Bill>(StaticClass.FootballFieldManagementDbContext);
        IRepository<ProductBill> _productBillRepository = new Repository<ProductBill>(StaticClass.FootballFieldManagementDbContext);
        public ICommand FieldCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        public ICommand SaveProductCommand { get; set; }
        public ICommand SaveBillCommand { get; set; }
        public ICommand PrintBillCommand { get; set; }
        public ICommand DestroyCommand { get; set; }
        public BillCalculatorViewModel()
        {
            LoadCombobox();
            Load();
            if (ListProduct.Count > 0)
            {
                CalculatorProduct();
            }
            StartCommand = new RelayCommand<object>(p =>
            {
                return true;
            }, async p =>
            {
                for (int i = 0; i < ListFieldType.Count; i++)
                {
                    ListField = new ObservableCollection<Field>(_fieldRepository.AsQueryable().Where(x => x.FieldTypeId == ListFieldType[i].Id).ToList());
                    for (int j = 0; j < ListField.Count; j++)
                    {
                        if (ListGroup[i].ListImage[j].ContentImage == FieldName)
                        {
                            ListGroup[i].ListImage[j].ImagePath = "D:\\Do An\\FootballFieldManagement\\FootballFieldManagement.UI\\Images\\do-book.png";
                            break;
                        }
                    }
                }
            });
            AddProductCommand = new RelayCommand<object>(p =>
            {
                if (FieldName == null)
                {
                    return false;
                }
                for (int i = 0; i < ListFieldType.Count; i++)
                {
                    ListField = new ObservableCollection<Field>(_fieldRepository.AsQueryable().Where(x => x.FieldTypeId == ListFieldType[i].Id).ToList());
                    for (int j = 0; j < ListField.Count; j++)
                    {
                        if (ListGroup[i].ListImage[j].ContentImage == FieldName)
                        {
                            if (ListGroup[i].ListImage[j].ImagePath != "D:\\Do An\\FootballFieldManagement\\FootballFieldManagement.UI\\Images\\do-book.png")
                                return false;
                        }
                    }
                }
                return true;
            }, p =>
            {
                ChildWindow childWindow = new ChildWindow();
                if (childWindow.ShowDialog() == false)
                {
                    Product product = new Product();
                    if (ListProduct == null)
                        ListProduct = new ObservableCollection<Product>();
                    product = childWindow.dataGridProduct.SelectedValue as Product;
                    if (product != null)
                    {
                        product.Quality = 1;
                        ListProduct.Add(product);
                        CalculatorProduct();
                    }
                }
            });
            SaveProductCommand = new RelayCommand<object>(p =>
            {
                if (SelectedProduct == null)
                {
                    return false;
                }
                return true;
            }, p =>
            {
                ObservableCollection<Product> products = new ObservableCollection<Product>(ListProduct);
                for (int i = 0; i < products.Count; i++)
                {
                    if (products[i].Code == SelectedProduct.Code)
                    {
                        products[i].Quality = int.Parse(Quality);
                    }
                }
                ListProduct = new ObservableCollection<Product>(products);
            });
            DestroyCommand = new RelayCommand<object>(p =>
            {
                return true;
            }, p =>
            {
                reset();
            });
            SaveBillCommand = new RelayCommand<object>(p =>
            {
                if (String.IsNullOrEmpty(Total) || String.IsNullOrEmpty(FieldPrice))
                    return false;
                return true;
            }, async p =>
            {
                var pricesQuery = _billRepository.AsQueryable().Where(x => x.DatePlay.Date == DatePlay.Date && x.Field.Name == FieldName).ToList();
                if (pricesQuery.Any() && (StaticClass.ConvertTimeToDecimal(StartTime) < pricesQuery.Max(x => StaticClass.ConvertTimeToDecimal(x.EndTime))
                || StaticClass.ConvertTimeToDecimal(EndTime) < pricesQuery.Min(x => StaticClass.ConvertTimeToDecimal(x.StartTime))))
                {
                    MessageBox.Show("Đã tồn tại hóa đơn của sân này rồi");
                }
                else
                {
                    Bill bill = new Bill()
                    {
                        CustomerId = SelectedCustomer.Id,
                        FieldId = _fieldRepository.AsQueryable().Where(x => x.Name == FieldName).FirstOrDefault().Id,
                        StartTime = StartTime,
                        EndTime = EndTime,
                        DatePlay = DatePlay,
                        Code = "MHD" + DateTime.Now.ToString("yyyyMMddHHmmssffff"),
                        PriceField = Double.Parse(FieldPrice),
                        PriceProduct = Double.Parse(ProductPrice),
                        Total = Double.Parse(Total),
                    };
                    bill = await _billRepository.AddAsync(bill);
                    for (int i = 0; i < ListProduct.Count; i++)
                    {
                        ProductBill productBill = new ProductBill()
                        {
                            ProductId = ListProduct[i].Id,
                            quantity = ListProduct[i].Quality,
                            BillId = _billRepository.AsQueryable().Where(x => x.StartTime == StartTime && x.EndTime == EndTime && x.DatePlay == DatePlay).FirstOrDefault().Id
                        };
                        productBill = await _productBillRepository.AddAsync(productBill);
                    }
                    if (bill != null)
                    {
                        MessageBox.Show("Lưu hóa đơn thành công");
                    }
                    else
                    {
                        MessageBox.Show("Lỗi hệ thống");
                    }
                }
            });
            PrintBillCommand = new RelayCommand<object>(p =>
            {
                return true;
            }, p =>
            {
                Report report = new Report();
                report.ShowDialog();
            });
            SelectedBookFieldCommand = new RelayCommand<object>(p =>
            {
                return true;
            }, p =>
            {
                LoadCombobox();
            });
        }
        private void LoadCombobox()
        {
            ListCustomer = new ObservableCollection<Customer>(_customerRepository.AsQueryable().ToList());
            ListFieldType = new ObservableCollection<FieldType>(_fieldTypeRepository.AsQueryable().ToList());
            SelectedFieldType = ListFieldType.FirstOrDefault();

        }
        private void Load()
        {
            ListGroup = new ObservableCollection<Group>();
            ListFieldType = new ObservableCollection<FieldType>(_fieldTypeRepository.AsQueryable().ToList());
            for (int i = 0; i < ListFieldType.Count; i++)
            {
                Group Group = new Group();
                Group.FieldType = ListFieldType[i].Name;
                ListField = new ObservableCollection<Field>(_fieldRepository.AsQueryable().Where(x => x.FieldTypeId == ListFieldType[i].Id).ToList());
                ObservableCollection<ImageItem> ListImage = new ObservableCollection<ImageItem>();
                for (int j = 0; j < ListField.Count; j++)
                {
                    ListImage.Add(new ImageItem
                    {
                        ImagePath = "D:\\Do An\\FootballFieldManagement\\FootballFieldManagement.UI\\Images\\no-book.png",
                        ContentImage = ListField[j].Name,
                        FieldCommand = new BillCalculatorCommand(),
                    });
                }
                Group.ListImage = ListImage;
                ListGroup.Add(Group);
            }
        }
        private void CalculatorProduct()
        {
            Double Total = 0;
            for (int i = 0; i < ListProduct.Count; i++)
            {
                Total += ListProduct[i].PriceOut * (int)ListProduct[i].Quality;
            }
            ProductPrice = Total.ToString();
        }
        private void CalculatorFieldPrice()
        {
            double tiensan;
            if (FieldName != null)
            {
                int FieldTypeId = _fieldRepository.AsQueryable().FirstOrDefault(x => x.Name == FieldName).FieldTypeId;
                if (_fieldPriceRepository.AsQueryable().Any(x => x.FieldTypeId == FieldTypeId))
                {
                    tiensan = _fieldPriceRepository.AsQueryable().Where(x => x.FieldTypeId == FieldTypeId).FirstOrDefault().Price;
                }
                else
                {
                    tiensan = 0;
                }
                FieldPrice = (tiensan * Double.Parse(PlayTime)).ToString();
            }
        }
        private void CalculatorPlayTime()
        {
            if (String.IsNullOrEmpty(StartTime) || String.IsNullOrEmpty(EndTime) || IsValidTime(StartTime) == false || IsValidTime(EndTime) == false)
            {
                PlayTime = "";
            }
            else
            {
                double starttime = StaticClass.ConvertTimeToDecimal(StartTime);
                double endtime = StaticClass.ConvertTimeToDecimal(EndTime);
                PlayTime = Math.Round((endtime - starttime), 2).ToString();
                CalculatorFieldPrice();
            }
        }
        private void CalculatorTotal()
        {
            if (FieldPrice != null && ProductPrice != null)
            {
                Total = (Double.Parse(FieldPrice) + Double.Parse(ProductPrice)).ToString();
            }
        }
        /*public void Validate()
        {
            if (StartTime != null && IsValidTime(StartTime) == false)
            {
                ErrorValidateStartTime = "Sai định dạng xy:zt";
            }
            else if (EndTime != null && IsValidTime(EndTime) == false)
            {
                ErrorValidateEndTime = "Sai định dạng xy:zt";
            }
            else
            {
                ErrorValidateStartTime = "";
                ErrorValidateEndTime = "";
            }
        }*/
        public static bool IsValidTime(string input)
        {
            Regex regex = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
            return regex.IsMatch(input);
        }
        private void reset()
        {
            for (int i = 0; i < ListFieldType.Count; i++)
            {
                ListField = new ObservableCollection<Field>(_fieldRepository.AsQueryable().Where(x => x.FieldTypeId == ListFieldType[i].Id).ToList());
                for (int j = 0; j < ListField.Count; j++)
                {
                    ListGroup[i].ListImage[j].ImagePath = "D:\\Do An\\FootballFieldManagement\\FootballFieldManagement.UI\\Images\\no-book.png";
                }
                FieldName = null;
                StartTime = null;
                EndTime = null;
                PlayTime = null;
                DatePlay = DateTime.Now;
                SelectedCustomer = null;
                ListProductBill = null;
                ListProduct = null;
                Quality = null;
                FieldPrice = null;
                ProductPrice = "0";
                Total = null;
            }
        }
    }
}

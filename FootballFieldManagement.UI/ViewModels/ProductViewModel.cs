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
    public class ProductViewModel : BaseViewModel
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }
        private string _code;

        public string Code
        {
            get { return _code; }
            set { _code = value; OnPropertyChanged(); }
        }
        private string _priceOut;

        public string PriceOut
        {
            get { return _priceOut; }
            set { _priceOut = value; OnPropertyChanged(); }
        }


        public ObservableCollection<Product> _listProduct;
        public ObservableCollection<Product> ListProduct { get => _listProduct; set { _listProduct = value; OnPropertyChanged(); } }
        public ObservableCollection<Unit> _listUnit;
        public ObservableCollection<Unit> ListUnit { get => _listUnit; set { _listUnit = value; OnPropertyChanged(); } }
        public ObservableCollection<Category> _listCategory;
        public ObservableCollection<Category> ListCategory { get => _listCategory; set { _listCategory = value; OnPropertyChanged(); } }
        public Unit _selectedUnit;
        public Unit SelectedUnit
        {
            get
            {
                return _selectedUnit;
            }
            set
            {
                _selectedUnit = value;
                OnPropertyChanged();
            }
        }
        public Category _selectedCategory;
        public Category SelectedCategory
        {
            get
            {
                return _selectedCategory;
            }
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }
        public Product _selectedProduct;
        public Product SelectedProduct
        {
            get
            {
                return _selectedProduct;
            }
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
                if(SelectedProduct != null)
                {
                    SelectedUnit = SelectedProduct.Unit;
                    SelectedCategory = SelectedProduct.Category;
                    PriceOut = SelectedProduct.PriceOut.ToString();
                    Code = SelectedProduct.Code;
                    Name = SelectedProduct.Name;
                }
            }
        }
        private IRepository<Product> _productRepository = new Repository<Product>(StaticClass.FootballFieldManagementDbContext);
        private IRepository<Unit> _unitRepository = new Repository<Unit>(StaticClass.FootballFieldManagementDbContext);
        private IRepository<Category> _categoryRepository = new Repository<Category>(StaticClass.FootballFieldManagementDbContext);
        public ICommand AddCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ProductViewModel()
        {
            LoadData();
            LoadComboBox();
            AddCommand = new RelayCommand<object>(p =>
            {
                if (string.IsNullOrEmpty(Code) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(PriceOut)
                || SelectedUnit == null || SelectedCategory == null)
                    return false;
                if (_productRepository.AsQueryable().Any(x => x.Code == Code))
                    return false;
                return true;
            }, async p =>
            {
                try
                {
                    var newProduct = new Product()
                    {
                        Name = Name,
                        Code = Code,
                        PriceOut = Double.Parse(PriceOut),
                        CategoryId = SelectedCategory.Id,
                        UnitId = SelectedUnit.Id,
                    };
                    newProduct = await _productRepository.AddAsync(newProduct);
                    if (newProduct != null)
                    {
                        MessageBox.Show("Thêm sản phẩm thành công");
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
            DeleteCommand = new RelayCommand<object>(p =>
            {
                if (SelectedProduct == null)
                    return false;
                return true;
            }, async p =>
            {
                try
                {
                    Product deleteProduct = _productRepository.AsQueryable().FirstOrDefault(x => x.Id == SelectedProduct.Id);
                    await _productRepository.DeleteAsync(deleteProduct);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            UpdateCommand = new RelayCommand<object>(p =>
            {
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Code) || string.IsNullOrEmpty(PriceOut)
                || SelectedCategory == null || SelectedUnit == null)
                    return false;
                if(SelectedProduct.Category != SelectedCategory || SelectedProduct.Unit != SelectedUnit || Code != SelectedProduct.Code)
                    return false;
                return true;
            }, async p =>
            {
                try
                {
                    var updateProduct = _productRepository.AsQueryable().FirstOrDefault(x => x.Id == SelectedProduct.Id);
                    updateProduct.Name = Name;
                    updateProduct.Code = Code;
                    updateProduct.PriceOut = Double.Parse(PriceOut);
                    updateProduct = await _productRepository.UpdateAsync(updateProduct);
                    if (updateProduct != null)
                    {
                        MessageBox.Show("Sửa sản phẩm thành công");
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
        private void LoadComboBox()
        {
            ListCategory = new ObservableCollection<Category>(_categoryRepository.AsQueryable().ToList());
            ListUnit = new ObservableCollection<Unit>(_unitRepository.AsQueryable().ToList());
            SelectedCategory = ListCategory.FirstOrDefault();
            SelectedUnit = ListUnit.FirstOrDefault();
        }
        private void LoadData()
        {
            ListProduct = new ObservableCollection<Product>(_productRepository.AsQueryable().ToList());
        }
    }
}

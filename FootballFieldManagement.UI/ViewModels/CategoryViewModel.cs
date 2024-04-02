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
    public class CategoryViewModel : BaseViewModel
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }
        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get
            {
                return _selectedCategory;
            }
            set
            {
                _selectedCategory = value;
                if (SelectedCategory != null)
                {
                    Name = SelectedCategory.Name;
                }
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Category> _listCategory;
        public ObservableCollection<Category> ListCategory { get => _listCategory; set { _listCategory = value; OnPropertyChanged(); } }
        public IRepository<Category> _categoryRepository = new Repository<Category>(StaticClass.FootballFieldManagementDbContext);
        public ICommand AddCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public CategoryViewModel()
        {
            LoadData();
            AddCommand = new RelayCommand<object>(p =>
            {
                if (String.IsNullOrEmpty(Name))
                    return false;
                return true;
            }, async p =>
            {
                try
                {
                    var newCategory = new Category()
                    {
                        Name = Name
                    };
                    newCategory = await _categoryRepository.AddAsync(newCategory);
                    if (newCategory != null)
                    {
                        MessageBox.Show("Thêm đơn vị thành công");
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
                if (SelectedCategory == null)
                    return false;
                return true;
            },
            async p =>
            {
                try
                {
                    var deleteCategory = _categoryRepository.AsQueryable().FirstOrDefault(x => x.Id == SelectedCategory.Id);
                    await _categoryRepository.DeleteAsync(deleteCategory);
                    LoadData();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            UpdateCommand = new RelayCommand<object>(p =>
            {
                if (string.IsNullOrEmpty(Name))
                    return false;
                return true;
            }, async p =>
            {
                try
                {
                    var updateCategory = _categoryRepository.AsQueryable().FirstOrDefault(x => x.Id == SelectedCategory.Id);
                    updateCategory.Name = Name;
                    updateCategory = await _categoryRepository.UpdateAsync(updateCategory);
                    if (updateCategory != null)
                    {
                        MessageBox.Show("Sửa thành công");
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
        private void LoadData()
        {
            ListCategory = new ObservableCollection<Category>(_categoryRepository.AsQueryable().ToList());
        }
    }
}

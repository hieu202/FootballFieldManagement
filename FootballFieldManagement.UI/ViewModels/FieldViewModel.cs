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
    public class FieldViewModel : BaseViewModel
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<FieldType> _listFieldType;
        public ObservableCollection<FieldType> ListFieldType { get => _listFieldType; set { _listFieldType = value; OnPropertyChanged(); } }
        public ObservableCollection<Field> _listField;
        public ObservableCollection<Field> ListField { get => _listField; set { _listField = value; OnPropertyChanged(); } }
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
        private Field _selectedField;
        public Field SelectedField
        {
            get
            {
                return _selectedField;
            }
            set
            {
                _selectedField = value;
                OnPropertyChanged();
                if (SelectedField != null)
                {
                    Name = SelectedField.Name;
                    SelectedFieldType = SelectedField.FieldType;
                }
            }
        }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public FieldViewModel()
        {
            LoadComboBox();
            LoadData();
            AddCommand = new RelayCommand<object>(p =>
            {
                if (_fieldRepository.AsQueryable().Any(x => x.Name == Name))
                    return false;
                if (String.IsNullOrEmpty(Name))
                    return false;
                return true;
            }, async p =>
            {
                try
                {
                    var newField = new Field()
                    {
                        Name = Name,
                        FieldTypeId = SelectedFieldType.Id,
                        Status = "0"
                    };
                    newField = await _fieldRepository.AddAsync(newField);
                    if (newField != null)
                    {
                        MessageBox.Show("Thêm sân thành công");
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

            UpdateCommand = new RelayCommand<object>(p =>
            {
                if (SelectedField != null && SelectedField.FieldType != SelectedFieldType)
                    return false;
                if (_fieldRepository.AsQueryable().Any(x => x.Name == Name))
                    return false;
                if (String.IsNullOrEmpty(Name)) return false;
                return true;
            }, async p =>
            {
                var update = _fieldRepository.AsQueryable().FirstOrDefault(x => x.Id == SelectedField.Id);
                update.Name = Name;
                try
                {
                    update = await _fieldRepository.UpdateAsync(update);
                    if (update != null)
                    {
                        MessageBox.Show("Sửa tên sân thành công");
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
            DeleteCommand = new RelayCommand<object>(p =>
            {
                if(SelectedField == null) return false;
                return true;
            }, async p =>
            {
                await _fieldRepository.DeleteAsync(SelectedField);
                MessageBox.Show("Xóa sân thành công");
                LoadData()
            });
        }
        private IRepository<FieldType> _fieldTypeRepository = new Repository<FieldType>(StaticClass.FootballFieldManagementDbContext);
        private IRepository<Field> _fieldRepository = new Repository<Field>(StaticClass.FootballFieldManagementDbContext);

        private void LoadComboBox()
        {
            ListFieldType = new ObservableCollection<FieldType>(_fieldTypeRepository.AsQueryable().ToList());
            SelectedFieldType = ListFieldType.FirstOrDefault();
        }
        private void LoadData()
        {
            ListField = new ObservableCollection<Field>(_fieldRepository.AsQueryable().OrderByDescending(x => x.FieldType.Name).ToList());
        }
    }
}

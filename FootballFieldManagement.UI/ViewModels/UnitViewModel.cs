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
    public class UnitViewModel : BaseViewModel
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }
        private Unit _selectedUnit;
        public Unit SelectedUnit
        {
            get
            {
                return _selectedUnit;
            }
            set
            {
                _selectedUnit = value;
                if (SelectedUnit != null)
                {
                    Name = SelectedUnit.Name;
                }
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Unit> _listUnit;
        public ObservableCollection<Unit> ListUnit { get => _listUnit; set { _listUnit = value; OnPropertyChanged(); } }
        public IRepository<Unit> _unitRepository = new Repository<Unit>(StaticClass.FootballFieldManagementDbContext);
        public ICommand AddCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public UnitViewModel()
        {
            LoadData();
            AddCommand = new RelayCommand<object>(p =>
            {
                if (String.IsNullOrEmpty(Name))
                    return false;
                if (_unitRepository.AsQueryable().Any(x => x.Name == Name))
                    return false;
                return true;
            }, async p =>
            {
                try
                {
                    var newUnit = new Unit()
                    {
                        Name = Name
                    };
                    newUnit = await _unitRepository.AddAsync(newUnit);
                    if (newUnit != null)
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
                if (SelectedUnit == null)
                    return false;
                return true;
            },
            async p =>
            {
                try
                {
                    var deleteUnit = _unitRepository.AsQueryable().FirstOrDefault(x => x.Id == SelectedUnit.Id);
                    await _unitRepository.DeleteAsync(deleteUnit);
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
                    var updateUnit = _unitRepository.AsQueryable().FirstOrDefault(x => x.Id == SelectedUnit.Id);
                    updateUnit.Name = Name;
                    updateUnit = await _unitRepository.UpdateAsync(updateUnit);
                    if (updateUnit != null)
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
            ListUnit = new ObservableCollection<Unit>(_unitRepository.AsQueryable().ToList());
        }
    }
}

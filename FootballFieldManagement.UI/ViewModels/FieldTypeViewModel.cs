using FootballFieldManagement.Core.Commands;
using FootballFieldManagement.Core.Repositories;
using FootballFieldManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace FootballFieldManagement.UI.ViewModels
{
    public class FieldTypeViewModel : BaseViewModel
    {
        private ObservableCollection<FieldType> _listFieldTypeView;
        public ObservableCollection<FieldType> ListFieldTypeView { get => _listFieldTypeView; set { _listFieldTypeView = value; OnPropertyChanged(); } }
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(Name);
            }
        }
        private string _numberOfPerson;
        public string NumberOfPerson
        {
            get => _numberOfPerson;
            set
            {
                _numberOfPerson = value;
                OnPropertyChanged(NumberOfPerson);
            }
        }
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(Description);
            }
        }
        public ICommand AddCommand { get; set; }
        public FieldTypeViewModel()
        {
            LoadData();
            AddCommand = new RelayCommand<object>(p =>
            {
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Description) || string.IsNullOrEmpty(NumberOfPerson))
                    return false;
                if (_fieldTypeRepository.AsQueryable().Any(x => x.Name == Name))
                    return false;
                return true;
            }, async p =>
            {
                try
                {
                    var newFieldType = new FieldType()
                    {
                        Name = Name,
                        Description = Description,
                        NumberPerson = NumberOfPerson,
                        Surcharge = "0"
                    };
                    newFieldType = await _fieldTypeRepository.AddAsync(newFieldType);
                    if(newFieldType != null)
                    {
                        MessageBox.Show("Thêm loại sân thành công");
                    } else
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
        private IRepository<FieldType> _fieldTypeRepository = new Repository<FieldType>(StaticClass.FootballFieldManagementDbContext);
        private void LoadData()
        {
            ListFieldTypeView = new ObservableCollection<FieldType>(_fieldTypeRepository.AsQueryable().ToList());
        }
    }
}

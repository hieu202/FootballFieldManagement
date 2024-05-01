using FootballFieldManagement.Core.Commands;
using FootballFieldManagement.Core.Repositories;
using FootballFieldManagement.Domain.Models;
using FootballFieldManagement.UI.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
            set { _listField = value; }
        }
        private ObservableCollection<FieldType> _listFieldType;

        public ObservableCollection<FieldType> ListFieldType
        {
            get { return _listFieldType; }
            set { _listFieldType = value; }
        }
        private ObservableCollection<Customer> _listCustomer;

        public ObservableCollection<Customer> ListCustomer
        {
            get { return _listCustomer; }
            set { _listCustomer = value; }
        }
        private ObservableCollection<Group> _listGroup;

        public ObservableCollection<Group> ListGroup
        {
            get { return _listGroup; }
            set { _listGroup = value; }
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
        public ICommand StartCommand { get; set; }
        IRepository<Field> _fieldRepository = new Repository<Field>(StaticClass.FootballFieldManagementDbContext);
        IRepository<FieldType> _fieldTypeRepository = new Repository<FieldType>(StaticClass.FootballFieldManagementDbContext);
        IRepository<Customer> _customerRepository = new Repository<Customer>(StaticClass.FootballFieldManagementDbContext);
        public ICommand FieldCommand { get; set; }
        public BillCalculatorViewModel()
        {
            LoadCombobox();
            Load();
            StartCommand = new RelayCommand<object>(p =>
            {
                return true;
            }, async p =>
            {
/*                for (int i = 0; i < ListField.Count; i++)
                {
                    if (ListImage[i].ContentImage == FieldName)
                    {
                        ListImage[i].ImagePath = "D:\\Do An\\FootballFieldManagement\\FootballFieldManagement.UI\\Images\\do-book.png";
                        break;
                    }
                }*/
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
    }
}

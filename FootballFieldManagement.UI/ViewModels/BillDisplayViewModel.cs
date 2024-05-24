using FootballFieldManagement.Core.Commands;
using FootballFieldManagement.Core.Repositories;
using FootballFieldManagement.Domain.Models;
using FootballFieldManagement.UI.Views;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
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
    public class BillDisplayViewModel : BaseViewModel
    {
        private ObservableCollection<Field> _listField;

        public ObservableCollection<Field> ListField
        {
            get { return _listField; }
            set { _listField = value; OnPropertyChanged(); }
        }
        private ObservableCollection<Customer> _listCustomer;

        public ObservableCollection<Customer> ListCustomer
        {
            get { return _listCustomer; }
            set { _listCustomer = value; OnPropertyChanged(); }
        }
        private ObservableCollection<Bill> _listBill;

        public ObservableCollection<Bill> ListBill
        {
            get { return _listBill; }
            set { _listBill = value; OnPropertyChanged(); }
        }
        private Customer _selectedCustomer;

        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set 
            {
                if( _selectedCustomer != value )
                {
                    _selectedCustomer = value;
                    LoadData();
                    OnPropertyChanged();

                }
            }
        }
        private Field _selectedField;

        public Field SelectedField
        {
            get { return _selectedField; }
            set
            {
                if (_selectedField != value)
                {
                    _selectedField = value; 
                    LoadData();
                    OnPropertyChanged();

                }
            }
        }
        private Bill _selectedBill;

        public Bill SelectedBill
        {
            get { return _selectedBill; }
            set { _selectedBill = value; OnPropertyChanged(); }
        }

        private DateTime _startDate = DateTime.Now;

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; OnPropertyChanged(); LoadData(); }
        }
        private DateTime _endDate = DateTime.Now;

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; OnPropertyChanged(); LoadData(); }
        }
        public ICommand DetailCommand { get; set; }
        public BillDisplayViewModel()
        {
            LoadCombobox();
            LoadData();
            DetailCommand = new RelayCommand<object>(p =>
            {
                if (_selectedBill == null)
                {
                    return false;
                }
                return true;
            }, p =>
            {
                Bill bill = _billRepository.AsQueryable().Where(x => x.Id == SelectedBill.Id).FirstOrDefault();
                List<ProductBill> productsBill = new List<ProductBill>(_productBillRepository.AsQueryable().Include(x => x.Product).ThenInclude(x => x.Category).Where(x => x.BillId == SelectedBill.Id).ToList());
                BillDetail billDetail = new BillDetail();
                billDetail.FieldName.Text = bill.Field.Name;
                billDetail.CustomerName.Text = bill.Customer.Name;
                billDetail.Code.Text = bill.Code;
                billDetail.FieldPrice.Text = bill.PriceField.ToString();
                billDetail.Date.Text = bill.DatePlay.Date.ToString();
                billDetail.StartTime.Text = bill.StartTime.ToString();
                billDetail.EndTime.Text = bill.EndTime.ToString();
                billDetail.ProductPrice.Text = bill.PriceProduct.ToString();
                billDetail.Total.Text = bill.Total.ToString();
                billDetail.datagridList.ItemsSource = productsBill.ToList();
                billDetail.ShowDialog();
            });
        }
        private readonly IRepository<Bill> _billRepository = new Repository<Bill>(StaticClass.FootballFieldManagementDbContext);
        private readonly IRepository<Customer> _customerRepository = new Repository<Customer>(StaticClass.FootballFieldManagementDbContext);
        private readonly IRepository<Field> _fieldRepository = new Repository<Field>(StaticClass.FootballFieldManagementDbContext);
        private readonly IRepository<ProductBill> _productBillRepository = new Repository<ProductBill>(StaticClass.FootballFieldManagementDbContext);
        private void LoadData()
        {
            //ListBill = new ObservableCollection<Bill>(_billRepository.AsQueryable().Include(x => x.Customer).Include(x => x.Field).OrderByDescending(x => x.DatePlay).ToList());
            var query = _billRepository.AsQueryable().Include(x => x.Customer).Include(x => x.Field).OrderByDescending(x => x.DatePlay) as IQueryable<Bill>;
            query = query.Where(x =>
                (SelectedCustomer == null || SelectedCustomer.Id == -1 || x.Customer.Name == SelectedCustomer.Name) &&
                (SelectedField == null || SelectedField.Id == -1 || x.Field.Name == SelectedField.Name) &&
                (StartDate == null || x.DatePlay.Date >= StartDate.Date) &&
                (EndDate == null || x.DatePlay.Date <= EndDate.Date));
            ListBill = new ObservableCollection<Bill>(query.ToList());
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
        }
        private void LoadCombobox()
        {
            ListCustomer = new ObservableCollection<Customer>(_customerRepository.AsQueryable().ToList());
            ListField = new ObservableCollection<Field>(_fieldRepository.AsQueryable().ToList());
        }
    }
}

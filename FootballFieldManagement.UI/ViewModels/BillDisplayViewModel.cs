using FootballFieldManagement.Core.Repositories;
using FootballFieldManagement.Domain.Models;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            set { _selectedCustomer = value; }
        }
        private Field _selectedField;

        public Field SelectedField
        {
            get { return _selectedField; }
            set { _selectedField = value; }
        }

        private string _startDate;

        public string StartDate
        {
            get { return _startDate; }
            set { _startDate = value; OnPropertyChanged(); }
        }
        private string _endDate;

        public string EndDate
        {
            get { return _endDate; }
            set { _endDate = value; OnPropertyChanged(); }
        }

        public BillDisplayViewModel()
        {
            LoadData();
            LoadCombobox();
        }
        private readonly IRepository<Bill> _billRepository = new Repository<Bill>(StaticClass.FootballFieldManagementDbContext);
        private readonly IRepository<Customer> _customerRepository = new Repository<Customer>(StaticClass.FootballFieldManagementDbContext);
        private readonly IRepository<Field> _fieldRepository = new Repository<Field>(StaticClass.FootballFieldManagementDbContext);
        private void LoadData()
        {
            ListBill = new ObservableCollection<Bill>(_billRepository.AsQueryable().Include(x => x.Customer).Include(x => x.Field).ToList());
        }
        private void LoadCombobox()
        {
            ListCustomer = new ObservableCollection<Customer>(_customerRepository.AsQueryable().ToList());
            SelectedCustomer = ListCustomer.FirstOrDefault();
            ListField = new ObservableCollection<Field>(_fieldRepository.AsQueryable().ToList());
            SelectedField = ListField.FirstOrDefault();
        }
    }
}

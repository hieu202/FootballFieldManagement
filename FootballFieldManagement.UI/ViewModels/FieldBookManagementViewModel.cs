using FootballFieldManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballFieldManagement.UI.ViewModels
{
    public class FieldBookManagementViewModel : BaseViewModel
    {
        private ObservableCollection<Customer> _listCustomer;
        public ObservableCollection<Customer> ListCustomer { get { return _listCustomer; } set { _listCustomer = value; OnPropertyChanged(); } }
        private ObservableCollection<Field> _listField;
        public ObservableCollection<Field> ListField { get { return _listField; } set { _listField = value; OnPropertyChanged(); } }
       
    }
}

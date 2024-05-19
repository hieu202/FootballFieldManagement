using FootballFieldManagement.Core.Commands;
using FootballFieldManagement.Core.Repositories;
using FootballFieldManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FootballFieldManagement.UI.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        private ObservableCollection<Customer> _listCustomer;
        public ObservableCollection<Customer> ListCustomer
        {
            get { return _listCustomer; }
            set { _listCustomer = value; OnPropertyChanged(); }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }
        private string _phone;

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; OnPropertyChanged(); }
        }
        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged(); }
        }
        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }
        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get
            {
                return _selectedCustomer;
            }
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
                if (SelectedCustomer != null)
                {
                    Name = SelectedCustomer.Name;
                    Phone = SelectedCustomer.Phone;
                    Address = SelectedCustomer.Address;
                    Email = SelectedCustomer.Email;
                }
            }
        }
        public IRepository<Customer> _customerRepository = new Repository<Customer>(StaticClass.FootballFieldManagementDbContext);
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public CustomerViewModel()
        {
            LoadData();
            AddCommand = new RelayCommand<object>(p =>
            {
                if (String.IsNullOrEmpty(Name) || String.IsNullOrEmpty(Email) || String.IsNullOrEmpty(Phone) || String.IsNullOrEmpty(Address))
                    return false;
                if (_customerRepository.AsQueryable().Any(x => x.Email == Email || x.Phone == Phone))
                    return false;
                return true;
            }, async p =>
            {
                try
                {
                    var newCustomer = new Customer()
                    {
                        Name = Name,
                        Phone = Phone,
                        Address = Address,
                        Email = Email
                    };
                    newCustomer = await _customerRepository.AddAsync(newCustomer);
                    if (newCustomer != null)
                    {
                        MessageBox.Show("Thêm khách hàng thành công");
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
                if (String.IsNullOrEmpty(Name) || String.IsNullOrEmpty(Email) || String.IsNullOrEmpty(Phone) || String.IsNullOrEmpty(Address))
                    return false;
                if (_customerRepository.AsQueryable().Any(x => x.Email == Email || x.Phone == Phone))
                    return false;
                if (SelectedCustomer == null)
                    return false;
                return true;
            }, async p =>
            {
                var updateCustomer = _customerRepository.AsQueryable().FirstOrDefault(x => x.Id == SelectedCustomer.Id);
                updateCustomer.Address = Address;
                updateCustomer.Email = Email;
                updateCustomer.Phone = Phone;
                updateCustomer.Name = Name;
                try
                {
                    updateCustomer = await _customerRepository.UpdateAsync(updateCustomer);
                    if (updateCustomer != null)
                    {
                        MessageBox.Show("Sửa khách hàng thành công");
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
                return true;
            }, async p =>
            {
                try
                {
                    var deleteCustomer = _customerRepository.AsQueryable().FirstOrDefault(x => x.Id == SelectedCustomer.Id);
                    await _customerRepository.DeleteAsync(deleteCustomer);
                    if (deleteCustomer != null)
                    {
                        MessageBox.Show("Xóa khách hàng thành công");
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
        }
        private void LoadData()
        {
            ListCustomer = new ObservableCollection<Customer>(_customerRepository.AsQueryable().ToList());
        }
    }
}

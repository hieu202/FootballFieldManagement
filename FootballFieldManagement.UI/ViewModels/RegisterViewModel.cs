using FootballFieldManagement.Core.Commands;
using FootballFieldManagement.Core.Repositories;
using FootballFieldManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace FootballFieldManagement.UI.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }
        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }
        private string _phone;
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }
        public ICommand LoginViewCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public IRepository<User> _repositoryUser = new Repository<User>(StaticClass.FootballFieldManagementDbContext);
        public RegisterViewModel()
        {
            LoginViewCommand = new RelayCommand<object>(p => true, p =>
            {
                StaticClass.Navigator.CurrentViewModel = new LoginViewModel();
            });
            RegisterCommand = new RelayCommand<object>(p =>
            {
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) ||
                string.IsNullOrEmpty(ConfirmPassword) || string.IsNullOrEmpty(Phone))
                    return false;
                return true;
            }, async (p) =>
            {
                try
                {
                    var user = await _repositoryUser.AsQueryable().FirstOrDefaultAsync(x => x.Email == Email);
                    if (user != null)
                    {
                        MessageBox.Show("Email đã tồn tại");
                    }
                    else if (Password.Length < 6)
                    {
                        MessageBox.Show("Mật khẩu phải dài hơn 6 ký tự");
                    }
                    else if (Password.CompareTo(ConfirmPassword) != 0)
                    {
                        MessageBox.Show("Mật khẩu phải giống xác nhận mật khẩu");
                    }
                    else
                    {
                        var newUser = new User()
                        {
                            UserName = UserName,
                            Email = Email,
                            Phone = Phone,
                            Password = Password,
                            Role = 1,
                        };
                        user = await _repositoryUser.AddAsync(newUser);
                        if(user != null)
                        {
                            MessageBox.Show("Thêm tài khoản thành công");
                            StaticClass.Navigator.CurrentViewModel = new LoginViewModel();
                        }
                        else
                        {
                            MessageBox.Show("Lỗi hệ thống");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }
    }
}

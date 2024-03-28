using FootballFieldManagement.Core.Commands;
using FootballFieldManagement.Core.Repositories;
using FootballFieldManagement.Domain.Models;
using FootballFieldManagement.UI.State;
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
    public class LoginViewModel : BaseViewModel
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
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterViewCommand
        {
            get
            {
                return new RelayCommand<object>(p => true, p =>
                {
                    StaticClass.Navigator.CurrentViewModel = new RegisterViewModel();
                });
            }
        }
        public IRepository<User> _repositoryUser = new Repository<User>(StaticClass.FootballFieldManagementDbContext);

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<object>(p =>
            {
                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                    return false;
                return true;
            }, async p =>
            {
                try
                {
                    StaticClass.UserStore.CurrentUser = await _repositoryUser.AsQueryable().FirstOrDefaultAsync(p => p.Email == Email);
                    if (StaticClass.UserStore.CurrentUser == null)
                    {
                        MessageBox.Show("Email không tồn tại");
                    }
                    else if (StaticClass.UserStore.CurrentUser.Password.CompareTo(Password) != 0)
                    {
                        MessageBox.Show("Mật khẩu không chính xác");
                    } else
                    { 
                        StaticClass.Navigator.CurrentViewModel = new MainContentViewModel();
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

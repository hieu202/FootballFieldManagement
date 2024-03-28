using FootballFieldManagement.Core.Commands;
using FootballFieldManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FootballFieldManagement.UI.ViewModels
{
    public class MainContentViewModel : BaseViewModel
    {
        public ICommand LogoutCommand { get; set; }
        public User CurrentUser
        {
            get
            {
                return StaticClass.UserStore.CurrentUser;
            }
            set
            {
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
        public MainContentViewModel()
        {
            LogoutCommand = new RelayCommand<object>(p => true, p =>
            {
                StaticClass.Navigator.CurrentViewModel = new LoginViewModel();
                StaticClass.UserStore.CurrentUser = null;
            });
        }
    }
}

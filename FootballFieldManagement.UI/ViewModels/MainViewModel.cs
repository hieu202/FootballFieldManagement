using FootballFieldManagement.UI.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballFieldManagement.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public BaseViewModel CurrentViewModel  => StaticClass.Navigator.CurrentViewModel;
        public MainViewModel()
        {
            StaticClass.Navigator.CurrentViewModelChanged += OnCurrenViewModelChanged;

            if (StaticClass.Navigator.CurrentViewModel == null)
            {
                StaticClass.Navigator.CurrentViewModel = new LoginViewModel();
            }

        }
        private void OnCurrenViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}

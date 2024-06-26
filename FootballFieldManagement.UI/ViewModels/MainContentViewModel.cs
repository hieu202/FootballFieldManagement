﻿using FootballFieldManagement.Core.Commands;
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
        public BaseViewModel CurrentViewModel => StaticClass.MainContent.CurrentViewModel;
        public ICommand LogoutCommand { get; set; }
        public ICommand FieldCommand { get; set; }
        public ICommand FieldTypeCommand { get; set; }
        public ICommand FieldPriceCommand { get; set; }
        public ICommand UnitCommand { get; set; }
        public ICommand CategoryCommand { get; set; }
        public ICommand ProductCommand { get; set; }
        public ICommand CustomerCommand { get; set; }
        public ICommand BookFieldCommand { get; set; }
        public ICommand DisplayFieldBookCommand { get; set; }
        public ICommand BillCalculatorCommand { get; set; }
        public ICommand BillDisplayCommand { get; set; }
        public ICommand DeleteFieldBookCommand { get; set; }
        public ICommand RevenueCommand { get; set; }
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
        private bool _isRole;

        public bool IsRole
        {
            get { return _isRole; }
            set { _isRole = value; }
        }

        public MainContentViewModel()
        {
            StaticClass.MainContent.CurrentViewModelChanged += OnCurrenViewModelChanged;
            Authentication();
            LogoutCommand = new RelayCommand<object>(p => true, p =>
            {
                StaticClass.Navigator.CurrentViewModel = new LoginViewModel();
                StaticClass.UserStore.CurrentUser = null;
            });
            FieldCommand = new RelayCommand<object>(p => true, p =>
            {
                StaticClass.MainContent.CurrentViewModel = new FieldViewModel();
            });
            FieldTypeCommand = new RelayCommand<object>(p => true, p =>
            {
                StaticClass.MainContent.CurrentViewModel = new FieldTypeViewModel();
            });
            FieldPriceCommand = new RelayCommand<object>(p => true, p =>
            {
                StaticClass.MainContent.CurrentViewModel = new FieldPriceViewModel();
            });
            UnitCommand = new RelayCommand<object>(p => true, p =>
            {
                StaticClass.MainContent.CurrentViewModel = new UnitViewModel();
            });
            CategoryCommand = new RelayCommand<object>(p => true, p =>
            {
                StaticClass.MainContent.CurrentViewModel = new CategoryViewModel();
            });
            ProductCommand = new RelayCommand<object>(p => true, p =>
            {
                StaticClass.MainContent.CurrentViewModel = new ProductViewModel();
            });
            CustomerCommand = new RelayCommand<object>(p => true, p =>
            {
                StaticClass.MainContent.CurrentViewModel = new CustomerViewModel();
            });
            BookFieldCommand = new RelayCommand<object>(p => true, p =>
            {
                StaticClass.MainContent.CurrentViewModel = new FieldBookManagementViewModel();
            });
            DisplayFieldBookCommand = new RelayCommand<object>(p => true, p =>
            {
                StaticClass.MainContent.CurrentViewModel = new DisplayFieldBookViewModel();
            });
            BillCalculatorCommand = new RelayCommand<object>(p => true, p =>
            {
                StaticClass.MainContent.CurrentViewModel = StaticClass.BillCalculatorViewModel;
            });
            BillDisplayCommand = new RelayCommand<object>(p => true, p =>
            {
                StaticClass.MainContent.CurrentViewModel = new BillDisplayViewModel();
            });
            DeleteFieldBookCommand = new RelayCommand<object>(p => true, p =>
            {
                StaticClass.MainContent.CurrentViewModel = new DeleteFieldBookViewModel();
            });
            RevenueCommand = new RelayCommand<object>(p => true, p =>
            {
                StaticClass.MainContent.CurrentViewModel = new RevenueViewModel();
            });

        }
        private void OnCurrenViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private void Authentication()
        {
            if(StaticClass.UserStore.CurrentUser.Role == 0)
            {
                IsRole = true;
            } else
            {
                IsRole = false;
            }
        }
    }
}

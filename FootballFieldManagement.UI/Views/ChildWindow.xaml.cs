using FootballFieldManagement.Core.Repositories;
using FootballFieldManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FootballFieldManagement.UI.Views
{
    /// <summary>
    /// Interaction logic for ChildWindow.xaml
    /// </summary>
    public partial class ChildWindow : Window
    {

        public ChildWindow()
        {
            InitializeComponent();
            LoadData();
        }
        IRepository<Product> _fieldRepository = new Repository<Product>(StaticClass.FootballFieldManagementDbContext);

        private void LoadData()
        {
            dataGridProduct.ItemsSource = new ObservableCollection<Product>(_fieldRepository.AsQueryable().Include(x => x.Unit).Include(x => x.Category).ToList());
        }

        private void dataGridProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

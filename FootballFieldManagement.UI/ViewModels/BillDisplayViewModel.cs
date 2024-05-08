using FootballFieldManagement.Core.Repositories;
using FootballFieldManagement.Domain.Models;
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
        private ObservableCollection<Bill> _listBill;

        public ObservableCollection<Bill> ListBill
        {
            get { return _listBill; }
            set { _listBill = value; OnPropertyChanged(); }
        }
        public BillDisplayViewModel()
        {
            LoadData();
        }
        private readonly IRepository<Bill> _billRepository = new Repository<Bill>(StaticClass.FootballFieldManagementDbContext);
        private void LoadData()
        {
            ListBill = new ObservableCollection<Bill>(_billRepository.AsQueryable().Include(x=> x.Customer).Include(x=>x.Field).ToList());
        }
    }
}

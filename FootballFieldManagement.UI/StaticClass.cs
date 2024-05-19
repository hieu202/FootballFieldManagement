using FootballFieldManagement.DbMigrator;
using FootballFieldManagement.Domain.Models;
using FootballFieldManagement.UI.State;
using FootballFieldManagement.UI.States;
using FootballFieldManagement.UI.ViewModels;
using FootballFieldManagement.UI.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballFieldManagement.UI
{
    public static class StaticClass
    {
        public static Navigator Navigator { get; set; } = new Navigator();
        public static MainContent MainContent { get; set; } = new MainContent();               
        public static FootballFieldManagementDbContext FootballFieldManagementDbContext { get; set; } = new FootballFieldManagementDbContext();
        public static UserStore UserStore { get; set; } = new UserStore();
        public static double ConvertTimeToDecimal(string time)
        {
            try
            {
                string[] parts = time.Split(':');
                int hours = int.Parse(parts[0]);
                int minutes = int.Parse(parts[1]);
                double decimalTime = hours + (minutes / 60.0);
                return decimalTime;
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid time format: " + e.Message);
                return -1; // Return -1 or any other value that indicates an error.
            }
        }
        public static BillCalculatorViewModel BillCalculatorViewModel { get; set; } = new BillCalculatorViewModel();
        public static SelectedBookFieldView SelectedBookFieldView { get; set; } = new SelectedBookFieldView();
        public static FieldBookManagement SelectedFieldBook { get; set; } = new FieldBookManagement();
    }
}

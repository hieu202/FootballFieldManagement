using FootballFieldManagement.DbMigrator;
using FootballFieldManagement.UI.State;
using FootballFieldManagement.UI.States;
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
    }
}

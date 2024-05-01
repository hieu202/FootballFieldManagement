using FootballFieldManagement.Core.Repositories;
using FootballFieldManagement.Domain.Models;
using FootballFieldManagement.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace FootballFieldManagement.UI.Commands
{
    public class BillCalculatorCommand : ICommand
    {
        IRepository<Field> _fieldRepository = new Repository<Field>(StaticClass.FootballFieldManagementDbContext);
        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            ObservableCollection<Field> ListField = new ObservableCollection<Field>(_fieldRepository.AsQueryable().ToList());
            for(int i = 0; i < ListField.Count; i++)
            {
                if (ListField[i].Name == parameter.ToString())
                {
                    StaticClass.BillCalculatorViewModel.FieldName = ListField[i].Name;
                }
            }
        }
    }
}

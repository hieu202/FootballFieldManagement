using FootballFieldManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballFieldManagement.Domain.Models
{
    public class Bill : DomainObject
    {
        public string Code {  get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }  
        public int FieldId { get; set; }
        public Field Field { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTime DatePlay {  get; set; }
        public double Total { get; set; }
        public double PriceField { get; set; }
        public double PriceProduct { get; set; }
        public ObservableCollection<ProductBill>? ProductBills { get; set; }
    }
}

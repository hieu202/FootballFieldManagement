using FootballFieldManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballFieldManagement.Domain.Models
{
    public class ProductBill : DomainObject
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int? quantity { get; set; }
        public int BillId {  get; set; }
        public Bill Bill { get; set; }
    }
}

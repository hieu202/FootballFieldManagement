using FootballFieldManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballFieldManagement.Domain.Models
{
    public class Product : DomainObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double PriceIn { get; set; }
        public double PriceOut { get; set;}

        public int UnitId {  get; set; }
        public Unit Unit { get; set; }
        public int CategoryId {  get; set; }
        public Category Category { get; set; }
    }
}

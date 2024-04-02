using FootballFieldManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballFieldManagement.Domain.Models
{
    public class FieldPrice : DomainObject
    {
        public double Price { get; set; }
        //public DateTime DateApply { get; set; }
        public int FieldTypeId { get; set; }
        public FieldType FieldType { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
    }
}

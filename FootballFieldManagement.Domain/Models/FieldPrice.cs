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
        public DateTime DateApply { get; set; }
        private int FieldTypeId { get; set; }
        private FieldType FieldType { get; set; }
        private int TimeId {  get; set; }
        private Time Time { get; set; }
    }
}

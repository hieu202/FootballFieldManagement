using FootballFieldManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballFieldManagement.Domain.Models
{
    public class FieldType : DomainObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string NumberPerson { get; set; }
        public string Surcharge { get; set; }
    }
}

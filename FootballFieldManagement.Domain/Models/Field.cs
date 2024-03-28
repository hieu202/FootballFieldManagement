using FootballFieldManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballFieldManagement.Domain.Models;
namespace FootballFieldManagement.Domain.Models
{
    public class Field : DomainObject
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public int FieldTypeId {  get; set; }
        public FieldType FieldType { get; set; }
    }
}

using FootballFieldManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballFieldManagement.Domain.Models
{
    public class FieldBookManagement : DomainObject
    {
        public int FieldId;
        public Field Field;
        public int CustomerId;
        public Customer Customer;
        public DateTime DateApply;
        public DateTime StartTime;
        public DateTime EndTime;
        public string Note;
    }
}

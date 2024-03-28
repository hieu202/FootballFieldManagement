using FootballFieldManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballFieldManagement.Domain.Models
{
    public class Date : DomainObject
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

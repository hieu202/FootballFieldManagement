using FootballFieldManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballFieldManagement.Domain.Models
{
    public class Time : DomainObject
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}

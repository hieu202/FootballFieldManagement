using FootballFieldManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballFieldManagement.Domain.Models
{
    public class Customer : DomainObject
    {
        public string Name { get; set; }
        public string Phone {  get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}

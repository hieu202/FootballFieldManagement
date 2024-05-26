using FootballFieldManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballFieldManagement.Domain.Models
{
    public class User : DomainObject
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone {  get; set; }
        public int Role {  get; set; }
    }
}

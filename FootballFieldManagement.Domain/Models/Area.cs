using FootballFieldManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballFieldManagement.Domain.Models
{
    public class Area : DomainObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

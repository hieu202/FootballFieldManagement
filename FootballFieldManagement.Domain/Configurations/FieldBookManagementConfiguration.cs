using FootballFieldManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballFieldManagement.Domain.Configurations
{
    public class FieldBookManagementConfiguration : IEntityTypeConfiguration<FieldBookManagement>
    {
        public void Configure(EntityTypeBuilder<FieldBookManagement> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}

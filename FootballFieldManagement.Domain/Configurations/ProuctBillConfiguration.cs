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
    public class ProuctBillConfiguration : IEntityTypeConfiguration<ProductBill>
    {
        public void Configure(EntityTypeBuilder<ProductBill> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}

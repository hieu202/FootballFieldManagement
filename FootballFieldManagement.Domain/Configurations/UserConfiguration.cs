using FootballFieldManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballFieldManagement.DbMigrator.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasData(new User()
            {
                Id = 1,
                UserName = "admin",
                Phone = "0123456",
                Email = "admin@gmail.com",
                Password = "123456",
                Role = 0,
            });
        }
    }
}

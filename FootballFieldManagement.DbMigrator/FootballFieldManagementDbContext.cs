using FootballFieldManagement.DbMigrator.Configurations;
using FootballFieldManagement.Domain.Configurations;
using FootballFieldManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballFieldManagement.DbMigrator
{
    public class FootballFieldManagementDbContext : DbContext
    {
        public DbSet<FieldType> FieldTypes { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Time> Times { get; set; }
        public DbSet<FieldPrice> FieldPrices { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<FieldBookManagement> FieldBookManagements { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"server=.;database=FootballFieldManagementDb;trusted_connection=true;TrustServerCertificate=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UnitConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new FieldTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FieldConfiguration());
            modelBuilder.ApplyConfiguration(new FieldPriceConfiguration());
            modelBuilder.ApplyConfiguration(new TimeConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new FieldBookManagementConfiguration());
        }
    }
}

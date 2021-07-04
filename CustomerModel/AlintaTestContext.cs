using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AlintaTestModels
{
    public class AlintaTestContext : DbContext
    {
        public AlintaTestContext(DbContextOptions<AlintaTestContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(
                b =>
                {
                    b.Property("Id");
                    b.HasKey("Id");
                    b.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                    b.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                    b.Property(e => e.DateOfBirth);
                });

            modelBuilder.Entity<Customer>()
                .HasIndex(p => p.FirstName);
            modelBuilder.Entity<Customer>()
                .HasIndex(p => p.LastName);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           // optionsBuilder.UseSqlServer(@"Data Source=(local);Initial Catalog=AlintaTest;Trusted_Connection=True");
        }

        public DbSet<Customer> Customers { get; set; }
    }
}

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
                    b.Property(e => e.FirstName).IsRequired();
                    b.Property(e => e.LastName).IsRequired();
                    b.Property(e => e.DateOfBirth);
                });
        }

        public DbSet<Customer> Customers { get; set; }
    }
}

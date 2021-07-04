using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlintaDomain;
using AlintaEF;
using Microsoft.EntityFrameworkCore;

namespace AlintaDatabaseTesting
{
    public class DatabaseFixture : IDatabaseFixture
    {
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        public DatabaseFixture()
        {
            Seed();
        }

        public virtual AlintaEFContext CreateContext(DbTransaction transaction = null)
        {
            return null;
        }

        /// <summary>
        /// Create base test data for all tests
        /// </summary>
        public void Seed()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        var customers = new List<Customer>();
                        var c = new Customer() {FirstName = "Rob", LastName = "Clayton", DateOfBirth = new DateTime(1976, 06, 19) };
                        customers.Add(c);
                        c = new Customer() { FirstName = "Jess", LastName = "Clayton", DateOfBirth = new DateTime(1989, 12, 27) };
                        customers.Add(c);
                        c = new Customer() { FirstName = "First1", LastName = "Last1", DateOfBirth = new DateTime(1920, 01, 01) };
                        customers.Add(c);
                        c = new Customer() { FirstName = "First2", LastName = "Last2", DateOfBirth = new DateTime(2010, 07, 4) };
                        customers.Add(c);
                        c = new Customer() { FirstName = "First3", LastName = "Last3", DateOfBirth = new DateTime(2001, 03, 07) };
                        customers.Add(c);
                        c = new Customer() { FirstName = "First4", LastName = "Last4" };
                        customers.Add(c);
                        c = new Customer() { FirstName = "Rob2", LastName = "Clayton2" };
                        customers.Add(c);
                        c = new Customer() { FirstName = "Rob3", LastName = "Clayton3" };
                        customers.Add(c);
                        c = new Customer() { FirstName = "Rob4", LastName = "Clayton4" };
                        customers.Add(c);


                        context.AddRange(customers);

                        context.SaveChanges();
                    }

                    _databaseInitialized = true;
                }
            }
        }
    }
}

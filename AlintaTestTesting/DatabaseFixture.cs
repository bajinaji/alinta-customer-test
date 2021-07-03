using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlintaTestModels;
using Microsoft.EntityFrameworkCore;

namespace AlintaTestTesting
{
    public class SharedDatabaseFixture : IDisposable
    {
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        public SharedDatabaseFixture()
        {
            Connection = new SqlConnection(@"Server=(localdb);Database=AlintaTest;Trusted_Connection=True;ConnectRetryCount=0");

            Seed();

            Connection.Open();
        }

        public DbConnection Connection { get; }

         public AlintaTestContext CreateContext(DbTransaction transaction = null)
        {
            var context =
                new AlintaTestContext(new DbContextOptionsBuilder<AlintaTestContext>().UseSqlServer(Connection).Options);

            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }

        private void Seed()
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
                        var c = new Customer() {FirstName = "Rob", LastName = "Clayton"};
                        customers.Add(c);
                        c = new Customer() { FirstName = "Rob", LastName = "Clayton" };
                        customers.Add(c);


                        context.AddRange(customers);

                        context.SaveChanges();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public void Dispose() => Connection.Dispose();
    }
}

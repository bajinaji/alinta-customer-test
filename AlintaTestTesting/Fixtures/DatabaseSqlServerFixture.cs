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
    public class DatabaseSqlServerFixture : DatabaseFixture
    {
        public DatabaseSqlServerFixture()
        {
        }

        public override AlintaTestContext CreateContext(DbTransaction transaction = null)
        {
            var context =
                new AlintaTestContext(new DbContextOptionsBuilder<AlintaTestContext>().UseSqlServer("Data Source=(local);Initial Catalog=AlintaTest;Trusted_Connection=True").Options);


            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }
    }
}

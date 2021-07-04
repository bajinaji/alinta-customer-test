using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlintaEF;
using Microsoft.EntityFrameworkCore;

namespace AlintaDatabaseTesting
{
    public class DatabaseSqlServerFixture : DatabaseFixture
    {
        public DatabaseSqlServerFixture()
        {
        }

        public override AlintaEFContext CreateContext(DbTransaction transaction = null)
        {
            var context =
                new AlintaEFContext(new DbContextOptionsBuilder<AlintaEFContext>().UseSqlServer("Data Source=(local);Initial Catalog=AlintaTest;Trusted_Connection=True").Options);


            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }
    }
}

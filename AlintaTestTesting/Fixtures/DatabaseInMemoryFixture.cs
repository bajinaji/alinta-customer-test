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
    public class DatabaseInMemoryFixture : DatabaseFixture
    {
        private readonly DbContextOptions<AlintaEFContext> _dbContextOptions = new DbContextOptionsBuilder<AlintaEFContext>()
            .UseInMemoryDatabase(databaseName: "AlintaTestDb")
            .Options;

        public DatabaseInMemoryFixture()
        {
        }

        public override AlintaEFContext CreateContext(DbTransaction transaction = null)
        {
            var context =
                new AlintaEFContext(_dbContextOptions);


            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }
    }
}

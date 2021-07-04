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
    public class DatabaseInMemoryFixture : DatabaseFixture
    {
        private readonly DbContextOptions<AlintaTestContext> _dbContextOptions = new DbContextOptionsBuilder<AlintaTestContext>()
            .UseInMemoryDatabase(databaseName: "AlintaTestDb")
            .Options;

        public DatabaseInMemoryFixture()
        {
        }

        public override AlintaTestContext CreateContext(DbTransaction transaction = null)
        {
            var context =
                new AlintaTestContext(_dbContextOptions);


            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }
    }
}

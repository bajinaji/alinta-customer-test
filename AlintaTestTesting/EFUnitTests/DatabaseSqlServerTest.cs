using System;
using System.Linq;
using AlintaTestModels;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;

namespace AlintaTestTesting
{
    public abstract class DatabaseSqlServerTest : DatabaseTest, IClassFixture<DatabaseSqlServerFixture>
    {
        public DatabaseSqlServerTest(DatabaseSqlServerFixture fixture) : base(fixture)
        {
        }
    }
}

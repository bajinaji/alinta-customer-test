using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;

namespace AlintaDatabaseTesting
{
    public abstract class DatabaseSqlServerTest : DatabaseTest, IClassFixture<DatabaseSqlServerFixture>
    {
        public DatabaseSqlServerTest(DatabaseSqlServerFixture fixture) : base(fixture)
        {
        }
    }
}

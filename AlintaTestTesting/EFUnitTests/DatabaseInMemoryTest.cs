using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;

namespace AlintaDatabaseTesting
{
    public class DatabaseInMemoryTest : DatabaseTest, IClassFixture<DatabaseInMemoryFixture>
    {
        public DatabaseInMemoryTest(DatabaseInMemoryFixture fixture) : base(fixture)
        {
        }
    }
}

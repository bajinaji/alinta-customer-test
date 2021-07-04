using System;
using System.Linq;
using AlintaTestModels;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;

namespace AlintaTestTesting
{
    public class DatabaseInMemoryTest : DatabaseTest, IClassFixture<DatabaseInMemoryFixture>
    {
        public DatabaseInMemoryTest(DatabaseInMemoryFixture fixture) : base(fixture)
        {
        }
    }
}

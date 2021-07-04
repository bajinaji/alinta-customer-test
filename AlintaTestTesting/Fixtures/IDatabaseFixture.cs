using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlintaTestModels;

namespace AlintaTestTesting
{
    interface IDatabaseFixture
    {
        AlintaTestContext CreateContext(DbTransaction transaction = null);
    }
}

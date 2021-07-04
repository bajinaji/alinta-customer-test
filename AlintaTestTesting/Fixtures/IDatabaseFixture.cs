using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlintaEF;

namespace AlintaDatabaseTesting
{
    interface IDatabaseFixture
    {
        AlintaEFContext CreateContext(DbTransaction transaction = null);
    }
}

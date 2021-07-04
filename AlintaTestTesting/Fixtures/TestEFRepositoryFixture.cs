using AlintaDomain;
using AlintaEF;
using Microsoft.EntityFrameworkCore;

namespace AlintaControllerTesting
{
    public class TestEFRepositoryFixture
    {
        public ICustomerRepository Repository { get; set; }
        public AlintaEFContext Context { get; set; }
        public TestEFRepositoryFixture()
        {
            DbContextOptions<AlintaEFContext> dbContextOptions = new DbContextOptionsBuilder<AlintaEFContext>()
                .UseInMemoryDatabase(databaseName: "AlintaEFRepositoryTestDb")
                .Options;
            Context =
                new AlintaEFContext(dbContextOptions);
            Repository = new CustomerTestEFRepository(Context);
        }
    }
}
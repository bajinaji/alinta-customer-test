using AlintaDomain;
using AlintaEF;
using Microsoft.EntityFrameworkCore;

namespace AlintaControllerTesting
{
    public class ControllerFixture
    {
        public ICustomerRepository Repository { get; set; }
        public AlintaEFContext Context { get; set; }
        public ControllerFixture()
        {
            DbContextOptions<AlintaEFContext> dbContextOptions = new DbContextOptionsBuilder<AlintaEFContext>()
                .UseInMemoryDatabase(databaseName: "AlintaEFRepositoryControllerTestDb")
                .Options;
            Context =
                new AlintaEFContext(dbContextOptions);
            Repository = new CustomerTestEFRepository(Context);
        }
    }
}
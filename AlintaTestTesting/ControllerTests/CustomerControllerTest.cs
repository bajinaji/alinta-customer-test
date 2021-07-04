using System;
using System.Data.Common;
using System.Linq;
using AlintaDatabaseTesting;
using AlintaDomain;
using AlintaEF;
using CustomerWebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace AlintaControllerTesting
{
    public class ControllerFixture
    {
        public ICustomerRepository Repository { get; set; }

        ControllerFixture()
        {
            DbContextOptions<AlintaEFContext> dbContextOptions = new DbContextOptionsBuilder<AlintaEFContext>()
                .UseInMemoryDatabase(databaseName: "AlintaTestDb")
                .Options;
            Repository = new CustomerRepositoryEF(new AlintaEFContext(dbContextOptions));
        }
    }

    public class ControllerTest:IClassFixture<ControllerFixture>
    {
        public ControllerFixture Fixture { get; set; }
        public ICustomerRepository Repository { get; set; }

        public ControllerTest(ControllerFixture fixture)
        {
            Fixture = fixture;
            Repository = fixture.Repository;
        }

        private CustomerController GetController()
        {
            return new CustomerController(new NullLogger<CustomerController>(), Repository);
        }

        [Theory]
        [InlineData("Rob", 4)]
        [InlineData("Rob2", 1)]
        [InlineData("zzzzz", 0)]
        public void SearchFirstName(string startsWith, int expectedResults)
        {
            var c = GetController();
            var customers = c.SearchFirstNameBeginsWith(startsWith).Result.ToArray();
            Assert.Equal(expectedResults, customers.Count());
            Assert.True(customers.Count(n => n.FirstName.StartsWith(startsWith)) == expectedResults);
        }

        [Fact]
        public void GetById()
        {
            var cont = GetController();
            var call = cont.GetById(1);

            var result = call.Result.Result as OkObjectResult;
            Assert.NotNull(call.Result);
            Assert.Equal(1, ((Customer)((OkObjectResult)call.Result.Result).Value).Id);

            var c = cont.GetById(0);
            Assert.True(c.Result.Result is BadRequestResult);
        }

        [Fact]
        public void PutExistingCustomer()
        {
            var c = GetController();
            var customer = c.SearchLastNameBeginsWith("Last4").Result.FirstOrDefault();
            Assert.NotNull(customer);
            Assert.NotNull(customer.Id);
            var id = (int)customer.Id;
            Assert.Null(customer.DateOfBirth);

            var dob = DateTime.Now.AddYears(-45);
            customer.DateOfBirth = dob;
            customer.FirstName = "ModifiedFirstName1";
            customer.LastName = "ModifiedLastName1";


            var r = c.Put(customer).Result;

            customer = ((Customer) ((OkObjectResult) c.GetById(id).Result.Result).Value);
            Assert.NotNull(customer);
            Assert.NotNull(customer.DateOfBirth);
            Assert.Equal(dob, customer.DateOfBirth);
            Assert.Equal("ModifiedFirstName1", customer.FirstName);
            Assert.Equal("ModifiedLastName1", customer.LastName);
        }

        [Fact]
        public void PostNewCustomer()
        {
            var c = GetController();

            var dob = DateTime.Now.AddYears(-45);
            var customer = new Customer() {DateOfBirth = dob, LastName = "NewCustomerLastName1", FirstName = "NewCustomerFirstName1" };
            Assert.Null(customer.Id);

            customer = ((Customer)((CreatedAtActionResult)c.Post(customer).Result.Result).Value);

            Assert.NotNull(customer);
            Assert.NotNull(customer.Id);
            Assert.NotNull(customer.DateOfBirth);
            Assert.Equal(dob, customer.DateOfBirth);
            Assert.Equal("NewCustomerFirstName1", customer.FirstName);
            Assert.Equal("NewCustomerLastName1", customer.LastName);
        }


        [Theory]
        [InlineData("Last", 4)]
        [InlineData("Clayton2", 1)]
        [InlineData("zzzzz", 0)]
        public void SearchLastName(string startsWith, int expectedResults)
        {
            var c = GetController();

            var customers = c.SearchLastNameBeginsWith(startsWith).Result.ToArray();
            Assert.Equal(expectedResults, customers.Count());
            Assert.True(customers.Count(n => n.LastName.StartsWith(startsWith)) == expectedResults);
        }
    }
}

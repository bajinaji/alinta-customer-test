using System;
using System.Data.Common;
using System.Linq;
using AlintaTestModels;
using CustomerWebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace AlintaTestTesting
{
    public class ControllerTest:IClassFixture<DatabaseInMemoryFixture>
    {
        public DatabaseInMemoryFixture Fixture { get; set; }

        public ControllerTest(DatabaseInMemoryFixture fixture)
        {
            Fixture = fixture;
        }

        [Theory]
        [InlineData("Rob", 4)]
        [InlineData("Rob2", 1)]
        [InlineData("zzzzz", 0)]
        public void SearchFirstName(string startsWith, int expectedResults)
        {
            var c = new CustomerController(new NullLogger<CustomerController>(), Fixture.CreateContext());
            var customers = c.SearchFirstNameBeginsWith(startsWith).ToArray();
            Assert.Equal(expectedResults, customers.Count());
            Assert.True(customers.Count(n => n.FirstName.StartsWith(startsWith)) == expectedResults);
        }

        [Fact]
        public void GetById()
        {
            var cont = new CustomerController(new NullLogger<CustomerController>(), Fixture.CreateContext());
            var call = cont.GetById(1);

            var result = call.Result as OkObjectResult;
            Assert.NotNull(call.Value);
            Assert.Equal(1, call.Value.Id);

            var c = cont.GetById(0);
            Assert.True(c.Result is BadRequestResult);
        }

        [Fact]
        public void PostExistingCustomer()
        {
            var c = new CustomerController(new NullLogger<CustomerController>(), Fixture.CreateContext());
            var customer = c.SearchLastNameBeginsWith("Last4").FirstOrDefault();
            Assert.NotNull(customer);
            Assert.NotNull(customer.Id);
            var id = (int)customer.Id;
            Assert.Null(customer.DateOfBirth);

            var dob = DateTime.Now.AddYears(-45);
            customer.DateOfBirth = dob;
            customer.FirstName = "ModifiedFirstName1";
            customer.LastName = "ModifiedLastName1";


            c.Post(customer);

            customer = c.GetById(id).Value;
            Assert.NotNull(customer);
            Assert.NotNull(customer.DateOfBirth);
            Assert.Equal(dob, customer.DateOfBirth);
            Assert.Equal("ModifiedFirstName1", customer.FirstName);
            Assert.Equal("ModifiedLastName1", customer.LastName);
        }

        [Fact]
        public void PostNewCustomer()
        {
            var c = new CustomerController(new NullLogger<CustomerController>(), Fixture.CreateContext());
            var dob = DateTime.Now.AddYears(-45);
            var customer = new Customer() {DateOfBirth = dob, LastName = "NewCustomerLastName1", FirstName = "NewCustomerFirstName1" };
            Assert.Null(customer.Id);

            customer = c.Post(customer).Value;

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
            var c = new CustomerController(new NullLogger<CustomerController>(), Fixture.CreateContext());
            var customers = c.SearchLastNameBeginsWith(startsWith).ToArray();
            Assert.Equal(expectedResults, customers.Count());
            Assert.True(customers.Count(n => n.LastName.StartsWith(startsWith)) == expectedResults);
        }
    }
}

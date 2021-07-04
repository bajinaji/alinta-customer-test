using System;
using System.Collections.Generic;
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
    public class CustomerTestEFRepository : CustomerRepositoryEF
    {
        public CustomerTestEFRepository(AlintaEFContext context) : base(context)
        {
            Seed();
        }

        public void Seed()
        {
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();

            var customers = new List<Customer>();
            var c = new Customer() {FirstName = "Rob", LastName = "Clayton", DateOfBirth = new DateTime(1976, 06, 19)};
            customers.Add(c);
            c = new Customer() {FirstName = "Jess", LastName = "Clayton", DateOfBirth = new DateTime(1989, 12, 27)};
            customers.Add(c);
            c = new Customer() {FirstName = "First1", LastName = "Last1", DateOfBirth = new DateTime(1920, 01, 01)};
            customers.Add(c);
            c = new Customer() {FirstName = "First2", LastName = "Last2", DateOfBirth = new DateTime(2010, 07, 4)};
            customers.Add(c);
            c = new Customer() {FirstName = "First3", LastName = "Last3", DateOfBirth = new DateTime(2001, 03, 07)};
            customers.Add(c);
            c = new Customer() {FirstName = "First4", LastName = "Last4"};
            customers.Add(c);
            c = new Customer() {FirstName = "Rob2", LastName = "Clayton2"};
            customers.Add(c);
            c = new Customer() {FirstName = "Rob3", LastName = "Clayton3"};
            customers.Add(c);
            c = new Customer() {FirstName = "Rob4", LastName = "Clayton4"};
            customers.Add(c);
            c = new Customer() { FirstName = "TheLastMe", LastName = "Clayton99" };
            customers.Add(c);


            Context.AddRange(customers);

            Context.SaveChanges();
        }
    }

    public class ControllerFixture
    {
        public ICustomerRepository Repository { get; set; }
        public AlintaEFContext Context { get; set; }
        public ControllerFixture()
        {
            DbContextOptions<AlintaEFContext> dbContextOptions = new DbContextOptionsBuilder<AlintaEFContext>()
                .UseInMemoryDatabase(databaseName: "AlintaControllerTestDb")
                .Options;
            Context =
                new AlintaEFContext(dbContextOptions);
            Repository = new CustomerTestEFRepository(Context);
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
            Assert.True(expectedResults == customers.Count());
            Assert.True(customers.Count(n => n.FirstName.StartsWith(startsWith)) == expectedResults);
        }

        [Fact]
        public void GetById()
        {
            var cont = GetController();
            var call = cont.GetById(1).Result;

            var result = call.Result as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(1, ((Customer)(result.Value)).Id);

            var c = cont.GetById(0).Result;
            Assert.True(c.Result is BadRequestResult);
        }

        [Fact]
        public void PutExistingCustomer()
        {
            var c = GetController();
            var customer = c.SearchLastNameBeginsWith("Clayton99").Result.FirstOrDefault();
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
            Assert.True(expectedResults == customers.Count());
            Assert.True(customers.Count(n => n.LastName.StartsWith(startsWith)) == expectedResults);
        }
    }
}

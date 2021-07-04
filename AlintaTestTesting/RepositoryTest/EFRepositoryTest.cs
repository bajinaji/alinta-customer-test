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
    public class EFRepositoryTest:IClassFixture<TestEFRepositoryFixture>
    {
        public TestEFRepositoryFixture Fixture { get; set; }
        public ICustomerRepository Repository { get; set; }

        public EFRepositoryTest(TestEFRepositoryFixture fixture)
        {
            Fixture = fixture;
            Repository = fixture.Repository;
        }

        [Theory]
        [InlineData("Rob", 4)]
        [InlineData("Rob2", 1)]
        [InlineData("zzzzz", 0)]
        public void SearchFirstName(string startsWith, int expectedResults)
        {
            var customers = Repository.GetCustomerByFirstNameBeginsWith(startsWith).Result.ToArray();
            Assert.True(expectedResults == customers.Count());
            Assert.True(customers.Count(n => n.FirstName.StartsWith(startsWith)) == expectedResults);
        }

        [Fact]
        public void GetById()
        {
            var customer = Repository.Get(1).Result;
            Assert.NotNull(customer);
            Assert.Equal(1, customer.Id);

            var c = Repository.Get(0).Result;
            Assert.Null(c);
        }

        [Fact]
        public void PutExistingCustomer()
        {
            var customer = Repository.GetCustomerByLastNameBeginsWith("Clayton99").Result.FirstOrDefault();
            Assert.NotNull(customer);
            Assert.NotNull(customer.Id);
            var id = (int)customer.Id;
            Assert.Null(customer.DateOfBirth);

            var dob = DateTime.Now.AddYears(-45);
            customer.DateOfBirth = dob;
            customer.FirstName = "ModifiedFirstName1";
            customer.LastName = "ModifiedLastName1";


            var r = Repository.UpdateAsync((customer)).Result;

            customer = Repository.Get(id).Result;
            Assert.NotNull(customer);
            Assert.NotNull(customer.DateOfBirth);
            Assert.Equal(dob, customer.DateOfBirth);
            Assert.Equal("ModifiedFirstName1", customer.FirstName);
            Assert.Equal("ModifiedLastName1", customer.LastName);
        }

        [Fact]
        public void PostNewValidCustomer()
        {
            var dob = DateTime.Now.AddYears(-45);
            var customer = new Customer() {DateOfBirth = dob, LastName = "NewCustomerLastName1", FirstName = "NewCustomerFirstName1" };
            Assert.Null(customer.Id);

            customer = Repository.AddAsync(customer).Result;

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
            var customers = Repository.GetCustomerByLastNameBeginsWith(startsWith).Result.ToArray();
            Assert.True(expectedResults == customers.Count());
            Assert.True(customers.Count(n => n.LastName.StartsWith(startsWith)) == expectedResults);
        }
    }
}

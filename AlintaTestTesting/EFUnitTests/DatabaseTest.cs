using System;
using System.Data.Common;
using System.Linq;
using Microsoft.Data.SqlClient;
using Xunit;
using AlintaDomain;
using AlintaEF;

namespace AlintaDatabaseTesting
{
    public abstract class DatabaseTest
    {
        public DatabaseFixture Fixture { get; set; }

        public DatabaseTest(DatabaseFixture fixture)
        {
            Fixture = fixture;
        }

        [Theory]
        [InlineData("Rob", 4)]
        [InlineData("Rob2", 1)]
        [InlineData("zzzzz", 0)]
        public void SearchFirstName(string startsWith, int expectedResults)
        {
            var context = Fixture.CreateContext();
            var customers = context.Customers.Where(c => c.FirstName.StartsWith(startsWith));
            Assert.Equal(expectedResults, customers.Count());
            Assert.True(customers.Count(n => n.FirstName.StartsWith(startsWith)) == expectedResults);
        }

        [Theory]
        [InlineData("Last", 4)]
        [InlineData("Clayton2", 1)]
        [InlineData("zzzzz", 0)]
        public void SearchLastName(string startsWith, int expectedResults)
        {
            var context = Fixture.CreateContext();
            var customers = context.Customers.Where(c => c.LastName.StartsWith(startsWith));
            Assert.Equal(expectedResults, customers.Count());
            Assert.True(customers.Count(n => n.LastName.StartsWith(startsWith)) == expectedResults);
        }

        [Fact]
        public void TestDefaultNullDateOfBirth()
        {
            int? id;
            using (var context = Fixture.CreateContext())
            {
                var customer = context.Customers.Add(new Customer()
                    {FirstName = "TestDateFirstName1", LastName = "TestDateLastName1"}).Entity;
                context.SaveChanges();
                id = customer.Id;
            }

            using (var context = Fixture.CreateContext())
            {
                var customer = context.Set<Customer>().First(e => e.Id == id);
                Assert.Null(customer.DateOfBirth);
                context.Customers.Remove(customer);
                context.SaveChanges();
            }
        }

        [Fact]
        public void TestValidDateOfBirth()
        {
            int? id;
            using (var context = Fixture.CreateContext())
            {
                var customer = context.Customers.Add(new Customer()
                    { FirstName = "TestDateFirstName1", LastName = "TestDateLastName1", DateOfBirth = new DateTime(1976, 06, 19)}).Entity;
                context.SaveChanges();
                id = customer.Id;
            }

            using (var context = Fixture.CreateContext())
            {
                var customer = context.Set<Customer>().First(e => e.Id == id);
                Assert.True(customer.DateOfBirth == new DateTime(1976, 06, 19));
                context.Customers.Remove(customer);
                context.SaveChanges();
            }
        }

        
        [Fact]
        public void CanAddCustomer()
        {
            var context = Fixture.CreateContext();
            var customer = context.Customers.Add(new Customer() {FirstName = "TestFirstName1", LastName = "TestLastName1"}).Entity;
            context.SaveChanges();
            Assert.Equal("TestFirstName1", customer.FirstName);
            Assert.Equal("TestLastName1", customer.LastName);
        }

        
        [Fact]
        public void CanDeleteCustomer()
        {
            int? id;
            using (var context = Fixture.CreateContext())
            {
                var customer = context.Customers.Add(new Customer()
                    {FirstName = "TestFirstName2", LastName = "TestLastName2"}).Entity;
                context.SaveChanges();
                id = customer.Id;
            }

            using (var context = Fixture.CreateContext())
            {
                var customer = context.Set<Customer>().First(e => e.Id == id);
                context.Customers.Remove(customer);
                context.SaveChanges();
                Assert.False(context.Set<Customer>().Any(e => e.Id == id));
                Assert.False(context.Set<Customer>().Any(e => e.FirstName == "TestFirstName2" && e.LastName == "TestLastName2"));

            }
        }

    }
}

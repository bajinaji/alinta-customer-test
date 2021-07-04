using System;
using System.Collections.Generic;
using AlintaDomain;
using AlintaEF;

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
}
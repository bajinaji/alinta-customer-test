using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlintaDomain;
using Microsoft.EntityFrameworkCore;

namespace AlintaEF
{
    public class CustomerRepositoryEF : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepositoryEF(AlintaEFContext context) : base(context)
        {
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<List<Customer>> GetCustomerByFirstNameBeginsWith(string beginsWith)
        {
            var customers = Context.Customers.Where(c => c.FirstName.StartsWith(beginsWith));
            return await customers.ToListAsync();
        }

        public async Task<List<Customer>> GetCustomerByLastNameBeginsWith(string beginsWith)
        {
            var customers = Context.Customers.Where(c => c.LastName.StartsWith(beginsWith));
            return await customers.ToListAsync();
        }
    }
}

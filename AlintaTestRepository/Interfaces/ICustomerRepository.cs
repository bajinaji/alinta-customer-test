using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlintaDomain
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<List<Customer>> GetCustomerByFirstNameBeginsWith(string beginsWith);
        Task<List<Customer>> GetCustomerByLastNameBeginsWith(string beginsWith);
    }
}

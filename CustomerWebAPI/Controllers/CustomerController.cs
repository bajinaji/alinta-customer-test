using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlintaTestModels;
using Microsoft.AspNetCore.Http;

namespace CustomerWebAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CustomerController : ControllerBase
	{
		private readonly ILogger<CustomerController> _logger;
        private readonly AlintaTestContext _context;

		public CustomerController(ILogger<CustomerController> logger, AlintaTestContext context)
		{
			_logger = logger;
            _context = context;
        }

		[HttpGet("SearchFirstNameBeginsWith")]
        public IEnumerable<Customer> SearchFirstNameBeginsWith(string beginsWith)
		{
            var customers = _context.Customers.Where(c => c.FirstName.StartsWith(beginsWith));
			return customers.ToArray();
		}

        
        [HttpGet("SearchLastNameBeginsWith")]
        public IEnumerable<Customer> SearchLastNameBeginsWith(string beginsWith)
        {
            var customers = _context.Customers.Where(c => c.LastName.StartsWith(beginsWith));
            return customers.ToArray();
        }

        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Customer> GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var customer = _context.Customers.Find(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        
        [HttpPost("Post")]
        public ActionResult<Customer> Post(Customer customer)
        {
            Customer saving;
            if (customer.Id != null)
            {
                saving = _context.Customers.Find(customer.Id);
                if (saving == null)
                {
                    return NotFound();
                }

            }
            else
            {
                saving = new Customer();
                _context.Customers.Add(saving);
            }

            saving.LastName = customer.LastName;
            saving.FirstName = customer.FirstName;
            saving.DateOfBirth = customer.DateOfBirth;
            _context.SaveChanges();

            return saving;
        }

        
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Customer>> Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var customerToDelete = await _context.Customers.FindAsync(id);

            if (customerToDelete == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customerToDelete);
            await _context.SaveChangesAsync();
            return customerToDelete;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AlintaDomain;
using Microsoft.AspNetCore.Http;

namespace CustomerWebAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CustomerController : ControllerBase
	{
		private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerRepository _customerRepository;

		public CustomerController(ILogger<CustomerController> logger, ICustomerRepository customerRepository)
		{
			_logger = logger;
            _customerRepository = customerRepository;
        }

		[HttpGet("SearchFirstNameBeginsWith")]
        [ProducesResponseType(typeof(IEnumerable<Customer>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<Customer>> SearchFirstNameBeginsWith(string beginsWith)
		{
            if (string.IsNullOrEmpty(beginsWith))
            {
                return new List<Customer>();
            }
            var customers = await _customerRepository.GetCustomerByFirstNameBeginsWith(beginsWith);
			return customers;
		}

        
        [HttpGet("SearchLastNameBeginsWith")]
        [ProducesResponseType(typeof(IEnumerable<Customer>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<Customer>> SearchLastNameBeginsWith(string beginsWith)
        {
            if (string.IsNullOrEmpty(beginsWith))
            {
                return new List<Customer>();
            }
            var customers = await _customerRepository.GetCustomerByLastNameBeginsWith(beginsWith);
            return customers;
        }

        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        // how to return the appropriate object type as the status ok
        public async Task<ActionResult<Customer>> GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var customer = await _customerRepository.Get(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }


        [HttpPut]
        // Update record
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Put(Customer customer)
        {
            if (customer.Id == null || customer.Id == 0)
            {
                return BadRequest();
            }

            await _customerRepository.UpdateAsync(customer);

            return NoContent();
        }

        [HttpPost]
        // New resource
        [ProducesResponseType(typeof(Customer), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(Customer customer)
        {
            if (customer.Id != null)
            {
                return BadRequest();
            }

            await _customerRepository.UpdateAsync(customer);

            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var foundAndDeleted = await _customerRepository.DeleteAsync(id);

            if (!foundAndDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

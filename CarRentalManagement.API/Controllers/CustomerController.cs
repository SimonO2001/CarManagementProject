// Controllers/CustomerController.cs
using Microsoft.AspNetCore.Mvc;
using CarRentalManagement.Repository.Interfaces;
using CarRentalManagement.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRentalManagement.API.Dtos;

namespace CarRentalManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCustomerDto>>> GetCustomers()
        {
            // Map the list of customers to list of GetCustomerDto
            var customers = await _customerRepository.GetAllCustomersAsync();
            var customerDtos = customers.Select(customer => new GetCustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                LicenseNumber = customer.LicenseNumber,
                Phone = customer.Phone,
                Email = customer.Email,
                Role = customer.Role
            });

            return Ok(customerDtos);
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCustomerDto>> GetCustomer(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            // Map the customer to GetCustomerDto
            var customerDto = new GetCustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                LicenseNumber = customer.LicenseNumber,
                Phone = customer.Phone,
                Email = customer.Email,
                Role = customer.Role
            };

            return customerDto;
        }

        // POST: api/Customer
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer([FromBody] CustomerCreateDto customerDto)
        {
            var customer = new Customer
            {
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                LicenseNumber = customerDto.LicenseNumber,
                Phone = customerDto.Phone,
                Email = customerDto.Email,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(customerDto.Password), // This is fine
                Role = customerDto.Role
            };

            await _customerRepository.AddCustomerAsync(customer, customerDto.Password); // Ensure this is correct
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }



        // PUT: api/Customer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            await _customerRepository.UpdateCustomerAsync(customer);
            return NoContent();
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _customerRepository.DeleteCustomerAsync(id);
            return NoContent();
        }
    }
}

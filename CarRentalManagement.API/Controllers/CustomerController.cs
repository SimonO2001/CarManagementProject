using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarRentalManagement.Repository.Interfaces;
using CarRentalManagement.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRentalManagement.API.Dtos;
using System.Linq;

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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<GetCustomerDto>>> GetCustomers()
        {
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

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetCustomerDto>> GetCustomer(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
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
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(customerDto.Password),
                Role = customerDto.Role
            };

            await _customerRepository.AddCustomerAsync(customer, customerDto.Password);
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutCustomer(int id, [FromBody] CustomerUpdateDto customerUpdateDto)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            if (customerUpdateDto.Password != null && !string.IsNullOrEmpty(customerUpdateDto.Password))
            {
                customer.HashedPassword = BCrypt.Net.BCrypt.HashPassword(customerUpdateDto.Password);
            }

            // Map other updatable fields
            customer.FirstName = customerUpdateDto.FirstName;
            customer.LastName = customerUpdateDto.LastName;
            customer.LicenseNumber = customerUpdateDto.LicenseNumber;
            customer.Phone = customerUpdateDto.Phone;
            customer.Email = customerUpdateDto.Email;
            customer.Role = customerUpdateDto.Role;

            await _customerRepository.UpdateCustomerAsync(customer);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            await _customerRepository.DeleteCustomerAsync(id);
            return NoContent();
        }
    }
}

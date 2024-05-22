using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarRentalManagement.Repository.Interfaces;
using CarRentalManagement.Repository.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CarRentalManagement.API.Dtos;
using Microsoft.Extensions.Logging;

namespace CarRentalManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalContractController : ControllerBase
    {
        private readonly IRentalContractRepository _rentalContractRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly ILogger<RentalContractController> _logger;

        public RentalContractController(
            IRentalContractRepository rentalContractRepository,
            ICustomerRepository customerRepository,
            IVehicleRepository vehicleRepository,
            IInsuranceRepository insuranceRepository,
            ILogger<RentalContractController> logger)
        {
            _rentalContractRepository = rentalContractRepository;
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
            _insuranceRepository = insuranceRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentalContract>>> GetRentalContracts()
        {
            return Ok(await _rentalContractRepository.GetAllRentalContractsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RentalContract>> GetRentalContract(int id)
        {
            var rentalContract = await _rentalContractRepository.GetRentalContractByIdAsync(id);

            if (rentalContract == null)
            {
                return NotFound();
            }

            return rentalContract;
        }

        [HttpPost]
        public async Task<ActionResult<RentalContract>> PostRentalContract([FromBody] RentalContractDto rentalContractDto)
        {
            _logger.LogInformation("Received rental contract data: {@RentalContractDto}", rentalContractDto);

            var customer = await _customerRepository.GetCustomerByIdAsync(rentalContractDto.CustomerId);
            var vehicle = await _vehicleRepository.GetVehicleByIdAsync(rentalContractDto.VehicleId);
            var insurance = await _insuranceRepository.GetInsuranceByIdAsync(rentalContractDto.InsuranceId);

            if (customer == null || vehicle == null || insurance == null)
            {
                _logger.LogWarning("Invalid customer, vehicle, or insurance.");
                return BadRequest("Invalid customer, vehicle, or insurance.");
            }

            var rentalContract = new RentalContract
            {
                VehicleId = rentalContractDto.VehicleId,
                CustomerId = rentalContractDto.CustomerId,
                InsuranceId = rentalContractDto.InsuranceId,
                StartDate = rentalContractDto.StartDate,
                EndDate = rentalContractDto.EndDate,
                TotalCost = rentalContractDto.TotalCost,
                Vehicle = vehicle,
                Customer = customer,
                Insurance = insurance
            };

            await _rentalContractRepository.AddRentalContractAsync(rentalContract);
            _logger.LogInformation("Rental contract created: {@RentalContract}", rentalContract);

            return CreatedAtAction(nameof(GetRentalContract), new { id = rentalContract.Id }, rentalContract);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutRentalContract(int id, [FromBody] RentalContractDto rentalContractDto)
        {
            var rentalContract = await _rentalContractRepository.GetRentalContractByIdAsync(id);
            if (rentalContract == null)
            {
                return NotFound();
            }

            var customer = await _customerRepository.GetCustomerByIdAsync(rentalContractDto.CustomerId);
            var vehicle = await _vehicleRepository.GetVehicleByIdAsync(rentalContractDto.VehicleId);
            var insurance = await _insuranceRepository.GetInsuranceByIdAsync(rentalContractDto.InsuranceId);

            if (customer == null || vehicle == null || insurance == null)
            {
                return BadRequest("Invalid customer, vehicle, or insurance.");
            }

            rentalContract.VehicleId = rentalContractDto.VehicleId;
            rentalContract.CustomerId = rentalContractDto.CustomerId;
            rentalContract.InsuranceId = rentalContractDto.InsuranceId;
            rentalContract.StartDate = rentalContractDto.StartDate;
            rentalContract.EndDate = rentalContractDto.EndDate;
            rentalContract.TotalCost = rentalContractDto.TotalCost;
            rentalContract.Vehicle = vehicle;
            rentalContract.Customer = customer;
            rentalContract.Insurance = insurance;

            await _rentalContractRepository.UpdateRentalContractAsync(rentalContract);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRentalContract(int id)
        {
            await _rentalContractRepository.DeleteRentalContractAsync(id);
            return NoContent();
        }

        [HttpGet("my-rentals")]
        [Authorize(Roles = "Default, Admin")]
        public async Task<ActionResult<IEnumerable<RentalContract>>> GetMyRentalContracts()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            _logger.LogInformation($"User ID from claims: {userIdClaim}");

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized();
            }

            if (int.TryParse(userIdClaim, out int parsedUserId))
            {
                var contracts = await _rentalContractRepository.GetRentalContractsByCustomerIdAsync(parsedUserId);
                return Ok(contracts);
            }
            else
            {
                return BadRequest("Invalid user ID.");
            }
        }
    }
}

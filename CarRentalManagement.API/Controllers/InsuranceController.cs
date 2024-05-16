using Microsoft.AspNetCore.Mvc;
using CarRentalManagement.Repository.Interfaces;
using CarRentalManagement.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentalManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceController : ControllerBase
    {
        private readonly IInsuranceRepository _insuranceRepository;

        public InsuranceController(IInsuranceRepository insuranceRepository)
        {
            _insuranceRepository = insuranceRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Insurance>>> GetInsurances()
        {
            return Ok(await _insuranceRepository.GetAllInsurancesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Insurance>> GetInsurance(int id)
        {
            var insurance = await _insuranceRepository.GetInsuranceByIdAsync(id);

            if (insurance == null)
            {
                return NotFound();
            }

            return insurance;
        }

        [HttpPost]
        public async Task<ActionResult<Insurance>> PostInsurance(Insurance insurance)
        {
            await _insuranceRepository.AddInsuranceAsync(insurance);
            return CreatedAtAction(nameof(GetInsurance), new { id = insurance.Id }, insurance);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutInsurance(int id, Insurance insurance)
        {
            if (id != insurance.Id)
            {
                return BadRequest();
            }

            await _insuranceRepository.UpdateInsuranceAsync(insurance);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInsurance(int id)
        {
            await _insuranceRepository.DeleteInsuranceAsync(id);
            return NoContent();
        }
    }
}

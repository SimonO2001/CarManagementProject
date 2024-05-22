using Microsoft.AspNetCore.Mvc;
using CarRentalManagement.Repository.Interfaces;
using CarRentalManagement.Repository.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRentalManagement.API.Dtos;

namespace CarRentalManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRecordController : ControllerBase
    {
        private readonly IServiceRecordRepository _serviceRecordRepository;

        public ServiceRecordController(IServiceRecordRepository serviceRecordRepository)
        {
            _serviceRecordRepository = serviceRecordRepository;
        }

        // GET: api/ServiceRecord
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceRecord>>> GetServiceRecords()
        {
            return Ok(await _serviceRecordRepository.GetAllServiceRecordsAsync());
        }

        // GET: api/ServiceRecord/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceRecord>> GetServiceRecord(int id)
        {
            var serviceRecord = await _serviceRecordRepository.GetServiceRecordByIdAsync(id);

            if (serviceRecord == null)
            {
                return NotFound();
            }

            return serviceRecord;
        }

        // POST: api/ServiceRecord
        [HttpPost]
        public async Task<ActionResult<ServiceRecord>> PostServiceRecord(ServiceRecordDto serviceRecordDto)
        {
            // Ensure the serviceRecordDto has a valid vehicleId and map it to the entity
            var serviceRecord = new ServiceRecord
            {
                VehicleId = serviceRecordDto.VehicleId,
                DateOfService = serviceRecordDto.DateOfService,
                Description = serviceRecordDto.Description,
                Cost = serviceRecordDto.Cost
            };

            await _serviceRecordRepository.AddServiceRecordAsync(serviceRecord);
            return CreatedAtAction("GetServiceRecord", new { id = serviceRecord.Id }, serviceRecord);
        }

        // PUT: api/ServiceRecord/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServiceRecord(int id, ServiceRecordDto serviceRecordDto)
        {
            if (id != serviceRecordDto.Id)
            {
                return BadRequest();
            }

            var serviceRecord = new ServiceRecord
            {
                Id = serviceRecordDto.Id,
                VehicleId = serviceRecordDto.VehicleId,
                DateOfService = serviceRecordDto.DateOfService,
                Description = serviceRecordDto.Description,
                Cost = serviceRecordDto.Cost
            };

            await _serviceRecordRepository.UpdateServiceRecordAsync(serviceRecord);
            return NoContent();
        }


        // DELETE: api/ServiceRecord/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceRecord(int id)
        {
            await _serviceRecordRepository.DeleteServiceRecordAsync(id);
            return NoContent();
        }

        // GET: api/ServiceRecord/vehicle/{vehicleId}
        [HttpGet("vehicle/{vehicleId}")]
        public async Task<ActionResult<IEnumerable<ServiceRecord>>> GetServiceRecordsByVehicleId(int vehicleId)
        {
            var serviceRecords = await _serviceRecordRepository.GetServiceRecordsByVehicleIdAsync(vehicleId);
            if (serviceRecords == null || !serviceRecords.Any())
            {
                return NotFound();
            }
            return Ok(serviceRecords);
        }
    }
}

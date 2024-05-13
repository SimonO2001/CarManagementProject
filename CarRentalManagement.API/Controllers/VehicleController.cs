using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarRentalManagement.Repository.Interfaces;
using CarRentalManagement.Repository.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;

namespace CarRentalManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly string _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        public VehicleController(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
            EnsureFolderExists(_imagePath);
        }

        private void EnsureFolderExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private string GenerateUniqueFileName(string originalFileName)
        {
            string extension = Path.GetExtension(originalFileName);
            return $"{Guid.NewGuid()}{extension}";
        }

        // GET: api/Vehicle
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
        {
            return Ok(await _vehicleRepository.GetAllVehiclesAsync());
        }

        // GET: api/Vehicle/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetVehicle(int id)
        {
            var vehicle = await _vehicleRepository.GetVehicleByIdAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return vehicle;
        }

        // POST: api/Vehicle
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult<Vehicle>> PostVehicle([FromForm] Vehicle vehicle, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is required.");
            }

            try
            {
                string uniqueFileName = GenerateUniqueFileName(file.FileName);
                var imagePath = Path.Combine(_imagePath, uniqueFileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Ensure the URL is correct
                vehicle.ImageUrl = $"{Request.Scheme}://{Request.Host.Value}/images/{uniqueFileName}";

                Console.WriteLine($"File path: {imagePath}");
                Console.WriteLine($"URL: {vehicle.ImageUrl}");

                await _vehicleRepository.AddVehicleAsync(vehicle);
                return CreatedAtAction(nameof(GetVehicle), new { id = vehicle.Id }, vehicle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Vehicle/5
        [HttpPut("{id}"), DisableRequestSizeLimit]
        public async Task<IActionResult> PutVehicle(int id, [FromForm] Vehicle vehicle, IFormFile file)
        {
            if (id != vehicle.Id)
            {
                return BadRequest();
            }

            if (file != null)
            {
                try
                {
                    string uniqueFileName = GenerateUniqueFileName(file.FileName);
                    var imagePath = Path.Combine(_imagePath, uniqueFileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    vehicle.ImageUrl = Path.Combine("images", uniqueFileName);

                    Console.WriteLine($"File path: {imagePath}");
                    Console.WriteLine($"URL: {vehicle.ImageUrl}");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            await _vehicleRepository.UpdateVehicleAsync(vehicle);
            return NoContent();
        }

        // DELETE: api/Vehicle/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            await _vehicleRepository.DeleteVehicleAsync(id);
            return NoContent();
        }
    }
}

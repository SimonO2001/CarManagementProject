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

        // Constructor to inject the repository dependency and ensure the image directory exists
        public VehicleController(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
            EnsureFolderExists(_imagePath);
        }

        // Ensure the folder exists or create it
        private void EnsureFolderExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        // Generate a unique file name for the uploaded image
        private string GenerateUniqueFileName(string originalFileName)
        {
            string extension = Path.GetExtension(originalFileName);
            return $"{Guid.NewGuid()}{extension}";
        }

        // GET: api/Vehicle
        // Retrieves all vehicles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
        {
            return Ok(await _vehicleRepository.GetAllVehiclesAsync());
        }

        // GET: api/Vehicle/5
        // Retrieves a specific vehicle by ID
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
        // Creates a new vehicle entry and uploads the associated image
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult<Vehicle>> PostVehicle([FromForm] Vehicle vehicle, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is required.");
            }

            try
            {
                // Save the uploaded file with a unique name
                string uniqueFileName = GenerateUniqueFileName(file.FileName);
                var imagePath = Path.Combine(_imagePath, uniqueFileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Set the image URL for the vehicle
                vehicle.ImageUrl = $"{Request.Scheme}://{Request.Host.Value}/images/{uniqueFileName}";

                Console.WriteLine($"File path: {imagePath}");
                Console.WriteLine($"URL: {vehicle.ImageUrl}");

                // Add the vehicle to the repository
                await _vehicleRepository.AddVehicleAsync(vehicle);
                return CreatedAtAction(nameof(GetVehicle), new { id = vehicle.Id }, vehicle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Vehicle/5
        // Updates an existing vehicle entry and optionally updates the associated image
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle(int id, [FromForm] Vehicle vehicle, IFormFile file)
        {
            if (id != vehicle.Id)
            {
                return BadRequest("Mismatched Vehicle ID.");
            }

            var existingVehicle = await _vehicleRepository.GetVehicleByIdAsync(id);
            if (existingVehicle == null)
            {
                return NotFound($"Vehicle with ID {id} not found.");
            }

            if (file != null)
            {
                try
                {
                    // Save the new uploaded file with a unique name
                    string uniqueFileName = GenerateUniqueFileName(file.FileName);
                    var imagePath = Path.Combine(_imagePath, uniqueFileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    existingVehicle.ImageUrl = $"{Request.Scheme}://{Request.Host.Value}/images/{uniqueFileName}";
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            // Update vehicle fields
            existingVehicle.Type = vehicle.Type;
            existingVehicle.Make = vehicle.Make;
            existingVehicle.Model = vehicle.Model;
            existingVehicle.Year = vehicle.Year;
            existingVehicle.VIN = vehicle.VIN;
            existingVehicle.Status = vehicle.Status;
            existingVehicle.CurrentMileage = vehicle.CurrentMileage;
            existingVehicle.RentalRate = vehicle.RentalRate;
            existingVehicle.HorsePower = vehicle.HorsePower;
            existingVehicle.Torque = vehicle.Torque;

            // Update the vehicle in the repository
            await _vehicleRepository.UpdateVehicleAsync(existingVehicle);
            return NoContent(); // Return 204 No Content to indicate successful update without a response body
        }

        // DELETE: api/Vehicle/5
        // Deletes a specific vehicle by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            await _vehicleRepository.DeleteVehicleAsync(id);
            return NoContent();
        }
    }
}

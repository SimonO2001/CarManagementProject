// Repositories/VehicleRepository.cs
using CarRentalManagement.Repository.Data;
using CarRentalManagement.Repository.Interfaces;
using CarRentalManagement.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentalManagement.Repository.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _context;

        // Constructor to inject the database context
        public VehicleRepository(AppDbContext context)
        {
            _context = context;
        }

        // Retrieves all vehicles from the database
        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await _context.Vehicles.ToListAsync();
        }

        // Retrieves a specific vehicle by ID from the database
        public async Task<Vehicle> GetVehicleByIdAsync(int id)
        {
            return await _context.Vehicles.FindAsync(id);
        }

        // Adds a new vehicle to the database
        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
        }

        // Updates an existing vehicle in the database
        public async Task UpdateVehicleAsync(Vehicle vehicle)
        {
            _context.Entry(vehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Deletes a specific vehicle by ID from the database
        public async Task DeleteVehicleAsync(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }
    }
}

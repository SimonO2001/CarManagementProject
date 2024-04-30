﻿// Repositories/RentalContractRepository.cs
using CarRentalManagement.Repository.Data;
using CarRentalManagement.Repository.Interfaces;
using CarRentalManagement.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentalManagement.Repository.Repositories
{
    public class RentalContractRepository : IRentalContractRepository
    {
        private readonly AppDbContext _context;

        public RentalContractRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RentalContract>> GetAllRentalContractsAsync()
        {
            return await _context.RentalContracts.Include(r => r.Vehicle).Include(r => r.Customer).ToListAsync();
        }

        public async Task<RentalContract> GetRentalContractByIdAsync(int id)
        {
            return await _context.RentalContracts.Include(r => r.Vehicle).Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddRentalContractAsync(RentalContract rentalContract)
        {
            _context.RentalContracts.Add(rentalContract);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRentalContractAsync(RentalContract rentalContract)
        {
            _context.Entry(rentalContract).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRentalContractAsync(int id)
        {
            var rentalContract = await _context.RentalContracts.FindAsync(id);
            if (rentalContract != null)
            {
                _context.RentalContracts.Remove(rentalContract);
                await _context.SaveChangesAsync();
            }
        }
    }
}

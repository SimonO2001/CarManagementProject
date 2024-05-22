using CarRentalManagement.Repository.Data;
using CarRentalManagement.Repository.Interfaces;
using CarRentalManagement.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalManagement.Repository.Repositories
{
    public class ServiceRecordRepository : IServiceRecordRepository
    {
        private readonly AppDbContext _context;

        public ServiceRecordRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ServiceRecord>> GetAllServiceRecordsAsync()
        {
            return await _context.ServiceRecords.ToListAsync();
        }

        public async Task<ServiceRecord> GetServiceRecordByIdAsync(int id)
        {
            return await _context.ServiceRecords.FindAsync(id);
        }

        public async Task AddServiceRecordAsync(ServiceRecord serviceRecord)
        {
            _context.ServiceRecords.Add(serviceRecord);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateServiceRecordAsync(ServiceRecord serviceRecord)
        {
            _context.Entry(serviceRecord).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteServiceRecordAsync(int id)
        {
            var serviceRecord = await _context.ServiceRecords.FindAsync(id);
            if (serviceRecord != null)
            {
                _context.ServiceRecords.Remove(serviceRecord);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ServiceRecord>> GetServiceRecordsByVehicleIdAsync(int vehicleId)
        {
            return await _context.ServiceRecords.Where(sr => sr.VehicleId == vehicleId).ToListAsync();
        }
    }
}

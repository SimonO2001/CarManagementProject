using CarRentalManagement.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentalManagement.Repository.Interfaces
{
    public interface IServiceRecordRepository
    {
        Task<IEnumerable<ServiceRecord>> GetAllServiceRecordsAsync();
        Task<ServiceRecord> GetServiceRecordByIdAsync(int id);
        Task AddServiceRecordAsync(ServiceRecord serviceRecord);
        Task UpdateServiceRecordAsync(ServiceRecord serviceRecord);
        Task DeleteServiceRecordAsync(int id);
        Task<IEnumerable<ServiceRecord>> GetServiceRecordsByVehicleIdAsync(int vehicleId); // New method
    }
}

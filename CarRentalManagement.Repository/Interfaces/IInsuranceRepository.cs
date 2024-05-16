// Interfaces/IInsuranceRepository.cs
using CarRentalManagement.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentalManagement.Repository.Interfaces
{
    public interface IInsuranceRepository
    {
        Task<IEnumerable<Insurance>> GetAllInsurancesAsync();
        Task<Insurance> GetInsuranceByIdAsync(int id);
        Task AddInsuranceAsync(Insurance insurance);
        Task UpdateInsuranceAsync(Insurance insurance);
        Task DeleteInsuranceAsync(int id);
    }
}

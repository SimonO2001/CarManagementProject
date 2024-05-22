// Interfaces/IRentalContractRepository.cs
using CarRentalManagement.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentalManagement.Repository.Interfaces
{
    public interface IRentalContractRepository
    {
        Task<IEnumerable<RentalContract>> GetAllRentalContractsAsync();
        Task<RentalContract> GetRentalContractByIdAsync(int id);
        Task AddRentalContractAsync(RentalContract rentalContract);
        Task UpdateRentalContractAsync(RentalContract rentalContract);
        Task DeleteRentalContractAsync(int id);
        Task<IEnumerable<RentalContract>> GetRentalContractsByCustomerIdAsync(int customerId); // Ensure it uses int
    }
}

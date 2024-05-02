// IRepository/ICustomerRepository.cs
using CarRentalManagement.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentalManagement.Repository.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int id);

        Task AddCustomerAsync(Customer customer, string password);


        Task<(bool isValid, string role)> CheckCredentialsAsync(string email, string password);

    }
}

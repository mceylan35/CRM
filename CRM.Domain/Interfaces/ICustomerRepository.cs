using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CRM.Domain.Common;
using CRM.Domain.Entities;

namespace CRM.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Result<Customer>> GetByIdAsync(Guid id);
        Task<Result<IReadOnlyList<Customer>>> GetAllAsync();
        Task<Result<IReadOnlyList<Customer>>> GetByNameAsync(string name);
        Task<Result<IReadOnlyList<Customer>>> GetByEmailAsync(string email);
        Task<Result<IReadOnlyList<Customer>>> GetByRegionAsync(string region);
        Task<Result<IReadOnlyList<Customer>>> GetByRegistrationDateRangeAsync(DateTime start, DateTime end);
        Task<Result<Customer>> AddAsync(Customer customer);
        Task<Result> UpdateAsync(Customer customer);
        Task<Result> DeleteAsync(Guid id);
    }
} 
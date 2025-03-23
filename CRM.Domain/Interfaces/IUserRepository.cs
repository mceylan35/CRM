using System.Threading.Tasks;
using CRM.Domain.Common;
using CRM.Domain.Entities;

namespace CRM.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<Result<User>> GetByIdAsync(System.Guid id);
        Task<Result<User>> GetByUsernameAsync(string username);
        Task<Result<System.Collections.Generic.IReadOnlyList<User>>> GetAllAsync();
        Task<Result<User>> AddAsync(User user);
        Task<Result> UpdateAsync(User user);
        Task<Result> DeleteAsync(System.Guid id);
    }
} 
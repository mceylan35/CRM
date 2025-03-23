using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CRM.Domain.Common;

namespace CRM.Domain.Interfaces
{
    public interface IRepositoryBase<T> where T : BaseEntity
    {
        Task<Result<T>> GetByIdAsync(Guid id);
        Task<Result<IReadOnlyList<T>>> GetAllAsync();
        Task<Result<IReadOnlyList<T>>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<Result<T>> AddAsync(T entity);
        Task<Result> UpdateAsync(T entity);
        Task<Result> DeleteAsync(Guid id);
    }
} 
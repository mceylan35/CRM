using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CRM.Domain.Common;
using CRM.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CRM.Persistence.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;
        protected readonly ILogger<RepositoryBase<T>> _logger;

        public RepositoryBase(ApplicationDbContext context, ILogger<RepositoryBase<T>> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<T>> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                
                if (entity == null)
                    return Result.Failure<T>($"ID'si {id} olan {typeof(T).Name} bulunamadı");
                
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{EntityName} ID'ye göre getirilirken hata oluştu. ID: {Id}", typeof(T).Name, id);
                return Result.Failure<T>($"Veritabanı hatası: {ex.Message}");
            }
        }

        public async Task<Result<IReadOnlyList<T>>> GetAllAsync()
        {
            try
            {
                var entities = await _dbSet.ToListAsync();
                return Result.Success<IReadOnlyList<T>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm {EntityName} verileri getirilirken hata oluştu", typeof(T).Name);
                return Result.Failure<IReadOnlyList<T>>($"Veritabanı hatası: {ex.Message}");
            }
        }

        public async Task<Result<IReadOnlyList<T>>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var entities = await _dbSet.Where(predicate).ToListAsync();
                return Result.Success<IReadOnlyList<T>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{EntityName} verileri filtrelenirken hata oluştu", typeof(T).Name);
                return Result.Failure<IReadOnlyList<T>>($"Veritabanı hatası: {ex.Message}");
            }
        }

        public async Task<Result<T>> AddAsync(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                await _context.SaveChangesAsync();
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yeni {EntityName} eklenirken hata oluştu", typeof(T).Name);
                return Result.Failure<T>($"Veritabanı hatası: {ex.Message}");
            }
        }

        public async Task<Result> UpdateAsync(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{EntityName} güncellenirken hata oluştu. ID: {Id}", typeof(T).Name, entity.Id);
                return Result.Failure($"Veritabanı hatası: {ex.Message}");
            }
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                
                if (entity == null)
                    return Result.Failure($"ID'si {id} olan {typeof(T).Name} bulunamadı");
                
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{EntityName} silinirken hata oluştu. ID: {Id}", typeof(T).Name, id);
                return Result.Failure($"Veritabanı hatası: {ex.Message}");
            }
        }
    }
} 
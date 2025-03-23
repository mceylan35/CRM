using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Domain.Common;
using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CRM.Persistence.Repositories
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context, ILogger<CustomerRepository> logger)
            : base(context, logger)
        {
        }

        public async Task<Result<IReadOnlyList<Customer>>> GetByNameAsync(string name)
        {
            try
            {
                var lowercaseName = name.ToLower();
                
                var customers = await _dbSet
                    .Where(c => c.FirstName.ToLower().Contains(lowercaseName) || 
                                c.LastName.ToLower().Contains(lowercaseName))
                    .ToListAsync();
                
                return Result.Success<IReadOnlyList<Customer>>(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İsme göre müşteri aranırken hata oluştu. İsim: {Name}", name);
                return Result.Failure<IReadOnlyList<Customer>>($"Veritabanı hatası: {ex.Message}");
            }
        }

        public async Task<Result<IReadOnlyList<Customer>>> GetByEmailAsync(string email)
        {
            try
            {
                var lowercaseEmail = email.ToLower();
                
                var customers = await _dbSet
                    .Where(c => c.Email.ToLower().Contains(lowercaseEmail))
                    .ToListAsync();
                
                return Result.Success<IReadOnlyList<Customer>>(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "E-postaya göre müşteri aranırken hata oluştu. E-posta: {Email}", email);
                return Result.Failure<IReadOnlyList<Customer>>($"Veritabanı hatası: {ex.Message}");
            }
        }

        public async Task<Result<IReadOnlyList<Customer>>> GetByRegionAsync(string region)
        {
            try
            {
                var lowercaseRegion = region.ToLower();
                
                var customers = await _dbSet
                    .Where(c => c.Region.ToLower().Contains(lowercaseRegion))
                    .ToListAsync();
                
                return Result.Success<IReadOnlyList<Customer>>(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bölgeye göre müşteri aranırken hata oluştu. Bölge: {Region}", region);
                return Result.Failure<IReadOnlyList<Customer>>($"Veritabanı hatası: {ex.Message}");
            }
        }

        public async Task<Result<IReadOnlyList<Customer>>> GetByRegistrationDateRangeAsync(DateTime start, DateTime end)
        {
            try
            {
                var customers = await _dbSet
                    .Where(c => c.RegistrationDate >= start && c.RegistrationDate <= end)
                    .ToListAsync();
                
                return Result.Success<IReadOnlyList<Customer>>(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kayıt tarihine göre müşteri aranırken hata oluştu. Başlangıç: {Start}, Bitiş: {End}", 
                    start, end);
                return Result.Failure<IReadOnlyList<Customer>>($"Veritabanı hatası: {ex.Message}");
            }
        }
    }
} 
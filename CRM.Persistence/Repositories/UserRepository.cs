using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CRM.Domain.Common;
using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CRM.Persistence.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
            : base(context, logger)
        {
        }

        public async Task<Result<User>> GetByUsernameAsync(string username)
        {
            try
            {
                var user = await _dbSet
                    .FirstOrDefaultAsync(u => u.Username == username);
                
                if (user == null)
                    return Result.Failure<User>($"Kullanıcı adı '{username}' olan kullanıcı bulunamadı");
                
                return Result.Success(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı adına göre kullanıcı aranırken hata oluştu. Kullanıcı adı: {Username}", username);
                return Result.Failure<User>($"Veritabanı hatası: {ex.Message}");
            }
        }
    }
} 
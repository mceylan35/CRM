using System;
using System.Threading.Tasks;
using CRM.Domain.Common.Extensions;
using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<SeedController> _logger;

        public SeedController(
            IUserRepository userRepository,
            ICustomerRepository customerRepository,
            ILogger<SeedController> logger)
        {
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SeedDatabase()
        {
            _logger.LogInformation("Veritabanı başlatma işlemi başladı");
            
            try
            {
                // Admin kullanıcısı ekle
                var adminPasswordHash = "admin123".HashPassword();
                var adminUser = new User("admin", adminPasswordHash, "Admin");
                await _userRepository.AddAsync(adminUser);
                
                // Normal kullanıcı ekle
                var userPasswordHash = "user123".HashPassword();
                var normalUser = new User("user", userPasswordHash, "User");
                await _userRepository.AddAsync(normalUser);
                
                // Örnek müşteriler ekle
                var customer1 = new Customer("John", "Doe", "john.doe@example.com", "North America");
                var customer2 = new Customer("Jane", "Smith", "jane.smith@example.com", "Europe");
                var customer3 = new Customer("Carlos", "Gomez", "carlos.gomez@example.com", "South America");
                
                await _customerRepository.AddAsync(customer1);
                await _customerRepository.AddAsync(customer2);
                await _customerRepository.AddAsync(customer3);
                
                _logger.LogInformation("Veritabanı başarıyla dolduruldu");
                
                return Ok(new { Success = true, Message = "Veritabanı başarıyla dolduruldu" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Veritabanı başlatma işlemi sırasında hata oluştu");
                return StatusCode(500, new { Success = false, Message = "Veritabanı başlatma işlemi sırasında hata oluştu: " + ex.Message });
            }
        }
    }
} 
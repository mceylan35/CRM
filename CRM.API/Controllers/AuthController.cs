using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CRM.Domain.Common;
using CRM.Domain.Common.Extensions;
using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserRepository userRepository, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class LoginResponse
        {
            public string Token { get; set; }
            public string Username { get; set; }
            public string Role { get; set; }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Giriş isteği alındı: {Username}", request.Username);
            
            // Kullanıcı adına göre kullanıcıyı getir
            var userResult = await _userRepository.GetByUsernameAsync(request.Username);
            
            if (userResult.IsFailure)
            {
                _logger.LogWarning("Başarısız giriş denemesi: Kullanıcı bulunamadı - {Username}", request.Username);
                return Unauthorized(Result.Failure("Kullanıcı adı veya şifre hatalı"));
            }

            var user = userResult.Value;
            
            // Şifre kontrolü (extension metodu kullanıyoruz)
            if (!request.Password.VerifyPassword(user.PasswordHash))
            {
                _logger.LogWarning("Başarısız giriş denemesi: Hatalı şifre - {Username}", request.Username);
                return Unauthorized(Result.Failure("Kullanıcı adı veya şifre hatalı"));
            }

            var token = GenerateJwtToken(user);
            
            _logger.LogInformation("Başarılı giriş: {Username}", request.Username);
            
            return Ok(new LoginResponse
            {
                Username = user.Username,
                Role = user.Role,
                Token = token
            });
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["Secret"]));
            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["DurationInMinutes"]));
            
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
} 
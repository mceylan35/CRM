using System;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace CRM.Domain.Common.Extensions
{
    /// <summary>
    /// Şifre hash'leme için extension metotları
    /// </summary>
    public static class PasswordHashExtensions
    {
        /// <summary>
        /// Şifreyi BCrypt kullanarak güvenli bir şekilde hash'ler
        /// </summary>
        /// <param name="password">Hash'lenecek şifre</param>
        /// <returns>Hash'lenmiş şifre</returns>
        public static string HashPassword(this string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), "Şifre boş olamaz");

            // BCrypt kullanarak hash'leme (salt otomatik oluşturulur)
            // Varsayılan WorkFactor: 10
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Verilen şifrenin hash ile eşleşip eşleşmediğini kontrol eder
        /// </summary>
        /// <param name="password">Kontrol edilecek şifre</param>
        /// <param name="storedHash">Depolanan hash değeri</param>
        /// <returns>Eşleşme durumu</returns>
        public static bool VerifyPassword(this string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password))
                return false;
                
            if (string.IsNullOrEmpty(storedHash))
                return false;
            
           
                
            try 
            {
                // BCrypt ile doğrulama
                return BCrypt.Net.BCrypt.Verify(password, storedHash);
            }
            catch
            { 
                 return false; 
            }
        }
    }
} 
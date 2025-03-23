using System;
using CRM.Domain.Common;

namespace CRM.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public string Role { get; private set; }

        private User() { }

        public User(string username, string passwordHash, string role)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            Role = role ?? throw new ArgumentNullException(nameof(role));
        }

       

       

        
    }
} 
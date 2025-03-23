using System;
using CRM.Domain.Common;

namespace CRM.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Region { get; private set; }
        public DateTime RegistrationDate { get; private set; }

        private Customer() { }

        public Customer(string firstName, string lastName, string email, string region)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Region = region ?? throw new ArgumentNullException(nameof(region));
            RegistrationDate = DateTime.UtcNow;
            
            ValidateEmail(email);
        }

        public void UpdateDetails(string firstName, string lastName, string email, string region)
        {
            if (!string.IsNullOrWhiteSpace(firstName))
                FirstName = firstName;
            
            if (!string.IsNullOrWhiteSpace(lastName))
                LastName = lastName;
            
            if (!string.IsNullOrWhiteSpace(email))
            {
                ValidateEmail(email);
                Email = email;
            }
            
            if (!string.IsNullOrWhiteSpace(region))
                Region = region;
                
            UpdatedAt = DateTime.UtcNow;
        }

        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("E-posta adresi boş olamaz", nameof(email));
                
            if (!email.Contains('@'))
                throw new ArgumentException("Geçerli bir e-posta adresi değil", nameof(email));
        }
        
        public string FullName => $"{FirstName} {LastName}";
    }
} 
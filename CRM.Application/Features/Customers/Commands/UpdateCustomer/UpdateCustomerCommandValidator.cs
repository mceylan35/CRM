using FluentValidation;

namespace CRM.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Müşteri ID'si gereklidir");
                
            RuleFor(c => c.FirstName)
                .NotEmpty().WithMessage("Ad alanı boş olamaz")
                .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir");
                
            RuleFor(c => c.LastName)
                .NotEmpty().WithMessage("Soyad alanı boş olamaz")
                .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir");
                
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("E-posta alanı boş olamaz")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz")
                .MaximumLength(100).WithMessage("E-posta en fazla 100 karakter olabilir");
                
            RuleFor(c => c.Region)
                .NotEmpty().WithMessage("Bölge alanı boş olamaz")
                .MaximumLength(50).WithMessage("Bölge en fazla 50 karakter olabilir");
        }
    }
} 
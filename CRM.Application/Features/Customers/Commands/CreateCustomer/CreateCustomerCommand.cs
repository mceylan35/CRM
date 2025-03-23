using CRM.Application.DTOs;
using CRM.Domain.Common;
using MediatR;

namespace CRM.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<Result<CustomerDto>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Region { get; set; }
    }
} 
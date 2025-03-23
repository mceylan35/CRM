using System;
using CRM.Domain.Common;
using MediatR;

namespace CRM.Application.Features.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest<Result>
    {
        public Guid Id { get; }
        
        public DeleteCustomerCommand(Guid id)
        {
            Id = id;
        }
    }
} 
using System;
using CRM.Application.DTOs;
using CRM.Domain.Common;
using MediatR;

namespace CRM.Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQuery : IRequest<Result<CustomerDto>>
    {
        public Guid Id { get; set; }
        
        public GetCustomerByIdQuery(Guid id)
        {
            Id = id;
        }
    }
} 
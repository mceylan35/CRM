using System.Collections.Generic;
using CRM.Application.DTOs;
using CRM.Domain.Common;
using MediatR;

namespace CRM.Application.Features.Customers.Queries.GetAllCustomers
{
    public class GetAllCustomersQuery : IRequest<Result<IReadOnlyList<CustomerDto>>>
    {
    }
} 
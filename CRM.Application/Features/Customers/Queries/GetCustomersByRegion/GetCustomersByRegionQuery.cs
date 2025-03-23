using System.Collections.Generic;
using CRM.Application.DTOs;
using CRM.Domain.Common;
using MediatR;

namespace CRM.Application.Features.Customers.Queries.GetCustomersByRegion
{
    public class GetCustomersByRegionQuery : IRequest<Result<IReadOnlyList<CustomerDto>>>
    {
        public string Region { get; set; }
        
        public GetCustomersByRegionQuery(string region)
        {
            Region = region;
        }
    }
} 
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CRM.Application.DTOs;
using CRM.Domain.Common;
using CRM.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRM.Application.Features.Customers.Queries.GetCustomersByRegion
{
    public class GetCustomersByRegionQueryHandler : IRequestHandler<GetCustomersByRegionQuery, Result<IReadOnlyList<CustomerDto>>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCustomersByRegionQueryHandler> _logger;

        public GetCustomersByRegionQueryHandler(ICustomerRepository customerRepository, IMapper mapper, ILogger<GetCustomersByRegionQueryHandler> logger)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IReadOnlyList<CustomerDto>>> Handle(GetCustomersByRegionQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Bölgeye göre müşteri bilgileri getiriliyor. Bölge: {Region}", request.Region);
            
            var customersResult = await _customerRepository.GetByRegionAsync(request.Region);
            
            if (customersResult.IsFailure)
            {
                _logger.LogWarning("Müşteri bilgileri getirilirken hata oluştu: {Error}", customersResult.Error);
                return Result.Failure<IReadOnlyList<CustomerDto>>(customersResult.Error);
            }

            var customerDtos = _mapper.Map<IReadOnlyList<CustomerDto>>(customersResult.Value);
            
            _logger.LogInformation("Bölgede toplam {Count} müşteri bilgisi başarıyla getirildi. Bölge: {Region}", 
                customerDtos.Count, request.Region);
                
            return Result.Success(customerDtos);
        }
    }
} 
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CRM.Application.DTOs;
using CRM.Domain.Common;
using CRM.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRM.Application.Features.Customers.Queries.GetAllCustomers
{
    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, Result<IReadOnlyList<CustomerDto>>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllCustomersQueryHandler> _logger;

        public GetAllCustomersQueryHandler(ICustomerRepository customerRepository, IMapper mapper, ILogger<GetAllCustomersQueryHandler> logger)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IReadOnlyList<CustomerDto>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Tüm müşteri bilgileri getiriliyor");
            
            var customersResult = await _customerRepository.GetAllAsync();
            
            if (customersResult.IsFailure)
            {
                _logger.LogWarning("Müşteri bilgileri getirilirken hata oluştu: {Error}", customersResult.Error);
                return Result.Failure<IReadOnlyList<CustomerDto>>(customersResult.Error);
            }

            var customerDtos = _mapper.Map<IReadOnlyList<CustomerDto>>(customersResult.Value);
            
            _logger.LogInformation("Toplam {Count} müşteri bilgisi başarıyla getirildi", customerDtos.Count);
            return Result.Success(customerDtos);
        }
    }
} 
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CRM.Application.DTOs;
using CRM.Domain.Common;
using CRM.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRM.Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Result<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCustomerByIdQueryHandler> _logger;

        public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository, IMapper mapper, ILogger<GetCustomerByIdQueryHandler> logger)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Müşteri bilgileri getiriliyor. Id: {CustomerId}", request.Id);
            
            var customerResult = await _customerRepository.GetByIdAsync(request.Id);
            
            if (customerResult.IsFailure)
            {
                _logger.LogWarning("Müşteri bulunamadı. Id: {CustomerId}, Hata: {Error}", 
                    request.Id, customerResult.Error);
                return Result.Failure<CustomerDto>(customerResult.Error);
            }

            var customerDto = _mapper.Map<CustomerDto>(customerResult.Value);
            
            _logger.LogInformation("Müşteri bilgileri başarıyla getirildi. Id: {CustomerId}", request.Id);
            return Result.Success(customerDto);
        }
    }
} 
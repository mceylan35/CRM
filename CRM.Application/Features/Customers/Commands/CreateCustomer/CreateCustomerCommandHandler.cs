using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CRM.Application.DTOs;
using CRM.Domain.Common;
using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRM.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCustomerCommandHandler> _logger;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper, ILogger<CreateCustomerCommandHandler> logger)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Yeni müşteri kaydı oluşturuluyor. E-posta: {Email}", request.Email);
            
            try
            {
                var customer = new Customer(
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.Region
                );
                
                var result = await _customerRepository.AddAsync(customer);
                
                if (result.IsFailure)
                {
                    _logger.LogError("Müşteri kaydı oluşturulurken hata oluştu: {Error}", result.Error);
                    return Result.Failure<CustomerDto>(result.Error);
                }
                
                var customerDto = _mapper.Map<CustomerDto>(result.Value);
                
                _logger.LogInformation("Müşteri kaydı başarıyla oluşturuldu. Id: {Id}", customerDto.Id);
                
                return Result.Success(customerDto);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Müşteri kaydı oluşturulurken beklenmeyen bir hata oluştu");
                return Result.Failure<CustomerDto>("Müşteri kaydı oluşturulurken bir hata oluştu. Hata: " + ex.Message);
            }
        }
    }
} 
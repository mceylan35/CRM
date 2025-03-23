using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CRM.Application.DTOs;
using CRM.Domain.Common;
using CRM.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRM.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCustomerCommandHandler> _logger;

        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper, ILogger<UpdateCustomerCommandHandler> logger)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<CustomerDto>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Müşteri bilgileri güncelleniyor. Id: {CustomerId}", request.Id);
            
            try
            {
                var customerResult = await _customerRepository.GetByIdAsync(request.Id);
                
                if (customerResult.IsFailure)
                {
                    _logger.LogWarning("Güncellenecek müşteri bulunamadı. Id: {CustomerId}", request.Id);
                    return Result.Failure<CustomerDto>($"ID'si {request.Id} olan müşteri bulunamadı");
                }
                
                var customer = customerResult.Value;
                
                customer.UpdateDetails(
                    request.FirstName, 
                    request.LastName, 
                    request.Email, 
                    request.Region
                );
                
                var updateResult = await _customerRepository.UpdateAsync(customer);
                
                if (updateResult.IsFailure)
                {
                    _logger.LogError("Müşteri güncellenirken hata oluştu: {Error}", updateResult.Error);
                    return Result.Failure<CustomerDto>(updateResult.Error);
                }
                
                // Güncellenmiş müşteriyi tekrar getir
                var updatedCustomerResult = await _customerRepository.GetByIdAsync(request.Id);
                if (updatedCustomerResult.IsFailure)
                {
                    return Result.Failure<CustomerDto>("Müşteri güncellendi ancak güncel bilgiler alınamadı");
                }
                
                var customerDto = _mapper.Map<CustomerDto>(updatedCustomerResult.Value);
                
                _logger.LogInformation("Müşteri bilgileri başarıyla güncellendi. Id: {CustomerId}", customerDto.Id);
                
                return Result.Success(customerDto);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Müşteri güncellenirken beklenmeyen bir hata oluştu. Id: {CustomerId}", request.Id);
                return Result.Failure<CustomerDto>("Müşteri güncellenirken bir hata oluştu. Hata: " + ex.Message);
            }
        }
    }
} 
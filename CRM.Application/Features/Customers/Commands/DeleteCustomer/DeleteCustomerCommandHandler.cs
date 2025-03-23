using System.Threading;
using System.Threading.Tasks;
using CRM.Domain.Common;
using CRM.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRM.Application.Features.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<DeleteCustomerCommandHandler> _logger;

        public DeleteCustomerCommandHandler(ICustomerRepository customerRepository, ILogger<DeleteCustomerCommandHandler> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Müşteri siliniyor. Id: {CustomerId}", request.Id);
            
            try
            {
                // Önce müşterinin var olduğundan emin ol
                var customerResult = await _customerRepository.GetByIdAsync(request.Id);
                
                if (customerResult.IsFailure)
                {
                    _logger.LogWarning("Silinecek müşteri bulunamadı. Id: {CustomerId}", request.Id);
                    return Result.Failure($"ID'si {request.Id} olan müşteri bulunamadı");
                }
                
                var deleteResult = await _customerRepository.DeleteAsync(request.Id);
                
                if (deleteResult.IsFailure)
                {
                    _logger.LogError("Müşteri silinirken hata oluştu: {Error}", deleteResult.Error);
                    return Result.Failure(deleteResult.Error);
                }
                
                _logger.LogInformation("Müşteri başarıyla silindi. Id: {CustomerId}", request.Id);
                
                return Result.Success();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Müşteri silinirken beklenmeyen bir hata oluştu. Id: {CustomerId}", request.Id);
                return Result.Failure("Müşteri silinirken bir hata oluştu. Hata: " + ex.Message);
            }
        }
    }
} 
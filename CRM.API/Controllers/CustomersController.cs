using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CRM.Application.DTOs;
using CRM.Application.Features.Customers.Commands.CreateCustomer;
using CRM.Application.Features.Customers.Commands.DeleteCustomer;
using CRM.Application.Features.Customers.Commands.UpdateCustomer;
using CRM.Application.Features.Customers.Queries.GetAllCustomers;
using CRM.Application.Features.Customers.Queries.GetCustomerById;
using CRM.Application.Features.Customers.Queries.GetCustomersByRegion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(IMediator mediator, ILogger<CustomersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CustomerDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllCustomersQuery());
            
            if (result.IsFailure)
                return BadRequest(result.Error);
            
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetCustomerByIdQuery(id));
            
            if (result.IsFailure)
                return NotFound(result.Error);
            
            return Ok(result.Value);
        }

        [HttpGet("region/{region}")]
        public async Task<ActionResult<IReadOnlyList<CustomerDto>>> GetByRegion(string region)
        {
            var result = await _mediator.Send(new GetCustomersByRegionQuery(region));
            
            if (result.IsFailure)
                return BadRequest(result.Error);
            
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Create(CreateCustomerCommand command)
        {
            var result = await _mediator.Send(command);
            
            if (result.IsFailure)
                return BadRequest(result.Error);
            
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> Update(Guid id, UpdateCustomerCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID yolu parametresi ve komut ID'si eşleşmiyor");
            
            var result = await _mediator.Send(command);
            
            if (result.IsFailure)
                return BadRequest(result.Error);
            
            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteCustomerCommand(id));
            
            if (result.IsFailure)
                return BadRequest(result.Error);
            
            return NoContent();
        }
    }
} 
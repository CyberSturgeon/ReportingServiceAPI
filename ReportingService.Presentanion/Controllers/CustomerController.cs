using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportingService.Application.Services.Interfaces;
using ReportingService.Core.Configuration;
using ReportingService.Core.Configuration.Filters;
using ReportingService.Presentanion.Models;

namespace ReportingService.Presentanion.Controllers;
[Route("api/customers")]
public class CustomerController(
    ICustomerService customerService,
    IMapper mapper) : Controller
{
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponse>> GetByIdAsync([FromRoute] Guid id)
    {
        var customer = mapper.Map<CustomerResponse>(
                       await customerService.GetByIdAsync(id));

        return Ok(customer);
    }

    [HttpGet("birth-date")]
    public async Task<ActionResult<List<CustomerResponse>>> GetByBirthAsync([FromQuery] DateFilter dates)
    {
        var customers = mapper.Map<List<CustomerResponse>>(
                       await customerService.GetByBirthAsync(dates));
        return Ok(customers);
    }
}

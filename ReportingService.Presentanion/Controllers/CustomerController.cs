using Microsoft.AspNetCore.Mvc;
using ReportingService.Core.Configuration;
using ReportingService.Presentanion.Models;

namespace ReportingService.Presentanion.Controllers;
[Route("api/customers")]
public class CustomerController : Controller
{
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponse>> GetCustomerByIdAsync([FromRoute] Guid id)
    {
        return Ok(new CustomerResponse());
    }

    [HttpGet("birth-date")]
    public async Task<ActionResult<ICollection<CustomerResponse>>> GetCustomersByBirthAsync([FromQuery] DateTime birth)
    {
        return Ok(new List<CustomerResponse>());
    }

    [HttpGet("{id}/transactions")]
    public async Task<ActionResult<CustomerResponse>> GetTransactionsByCustomerIdAsync(
            [FromRoute] Guid Id, [FromQuery] int? monthCount)
    {
        return Ok(new CustomerResponse());
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<CustomerResponse>>> GetCustomersByFilterdAsync([FromQuery] CustomerFilterRequest request)
    {
        return Ok(new List<CustomerResponse>());
    }
}

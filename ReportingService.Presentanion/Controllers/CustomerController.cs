using Microsoft.AspNetCore.Mvc;
using ReportingService.Presentanion.Models;

namespace ReportingService.Presentanion.Controllers;
[Route("api/customers")]
public class CustomerController : Controller
{
    [HttpGet]
    public async Task<ActionResult<CustomerResponse>> GetCustomerByIdAsync([FromQuery] Guid id)
    {
        return Ok(new CustomerResponse());
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetCustomersByBirthAsync([FromQuery] DateTime birth)
    {
        return Ok(new List<CustomerResponse>());
    }

    [HttpGet]
    public async Task<ActionResult<CustomerResponse>> GetCustomerByAccountIdAsync([FromQuery] Guid accountId)
    {
        return Ok(new CustomerResponse());
    }

    [HttpGet]
    public async Task<ActionResult<CustomerResponse>> GetCustomerByTransactionIdAsync([FromQuery] Guid transactionId)
    {
        return Ok(new CustomerResponse());
    }
}

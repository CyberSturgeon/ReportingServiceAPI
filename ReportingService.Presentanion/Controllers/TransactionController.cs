using Microsoft.AspNetCore.Mvc;
using ReportingService.Presentanion.Models;
//GET (monthCount transCount) => List<Guid> Ids
//GET (monthCount money) => List<Guid> Ids
// GET (day month) => List<Guid> Ids
// 
namespace ReportingService.Presentanion.Controllers;

[Route("api/transactions")]

public class TransactionController : Controller
{
    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionResponse>> GetTransactionByIdAsync([FromRoute] Guid id)
    {
        return Ok(new TransactionResponse());
    }

    [HttpGet("{id}/customer")]
    public async Task<ActionResult<CustomerResponse>> GetCustomerByTransactionIdAsync([FromRoute] Guid id)
    {
        return Ok(new CustomerResponse());
    }
}

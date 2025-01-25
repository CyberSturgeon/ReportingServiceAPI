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
    [HttpGet]
    public async Task<ActionResult<List<TransactionResponse>>> GetTransactionsByCustomerIdAsync(
            [FromQuery] Guid customerId, [FromQuery] int? monthCount)
    {

        return Ok(new List<TransactionResponse>());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionResponse>> GetTransactionByIdAsync(Guid id)
    {
        return Ok(new TransactionResponse());
    }

    [HttpGet]
    public async Task<ActionResult<List<TransactionResponse>>> GetTransactionsByAccountIdAsync(
            [FromQuery] Guid customerId, [FromQuery] int? monthCount)
    {

        return Ok(new List<TransactionResponse>());
    }
}

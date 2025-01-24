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
    public async Task<List<TransactionResponse>> GetTransactionsByCustomerId([FromQuery] Guid customerId, [FromQuery] int? monthCount)
    {

        return new List<TransactionResponse>();
    }

    [HttpGet("{id}")]
    public async Task<TransactionResponse> GetTransactionById(Guid id)
    {
        return new TransactionResponse();
    }


}

using Microsoft.AspNetCore.Mvc;
using ReportingService.Persistence.Entities;
using ReportingService.Presentanion.Models;

namespace ReportingService.Presentanion.Controllers;

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

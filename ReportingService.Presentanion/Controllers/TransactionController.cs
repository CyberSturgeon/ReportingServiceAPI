using Microsoft.AspNetCore.Mvc;
using ReportingService.Application.Services;
using ReportingService.Core.Configuration;
using ReportingService.Presentanion.Models;
//GET (monthCount transCount) => List<Guid> Ids
//GET (monthCount money) => List<Guid> Ids
// GET (day month) => List<Guid> Ids
// 
namespace ReportingService.Presentanion.Controllers;

[Route("api/customers")]

public class TransactionController(
    TransactionService transactionService) : Controller
{
    [HttpGet]
    public async Task<List<TransactionResponse>> GetAllTransactionsByCustomerId([FromQuery] Guid customerId, [FromQuery] int? monthCount)
    {
        var transactions = await transactionService.GetAllTransactionsByCustomerId(customerId);

        var response = transactions
            .Select(transaction => new TransactionResponse()
          {
              Id = transaction.Id,
              CustomerId = transaction.CustomerId,
              AccountId = transaction.AccountId,
              Amount = transaction.Amount,
              Date = transaction.Date,
              TransactionType = transaction.TransactionType,
              Currency = transaction.Currency,
          })
          .ToList();

        return response;
    }

    [HttpGet("{id}")]
    public async Task<TransactionResponse> GetTransactionById(Guid id)
    {
        return new TransactionResponse();
    }


}

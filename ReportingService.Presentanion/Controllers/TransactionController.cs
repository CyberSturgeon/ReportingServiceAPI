using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportingService.Application.Models;
using ReportingService.Application.Services;
using ReportingService.Core;
using ReportingService.Core.Configuration;
using ReportingService.Presentanion.Models;
//GET (monthCount transCount) => List<Guid> Ids
//GET (monthCount money) => List<Guid> Ids
// GET (day month) => List<Guid> Ids
// 
namespace ReportingService.Presentanion.Controllers;

[Route("api/transactions")]
public class TransactionController(
    TransactionService transactionService,
    IMapper mapper) : Controller
{
    [HttpPost]
    public async Task<List<TransactionResponse>> SearchTransactions(
        [FromBody] Guid customerId, 
        [FromBody] TransactionSearchFilter request)
    {
        var transactions = await transactionService.SearchTransaction(customerId, request.DateFrom, request.DateTo);

        var response = mapper.Map<List<TransactionResponse>>(transactions);
            
        return response;
    }

    [HttpGet("{id}")]
    public async Task<TransactionResponse> GetTransactionById(Guid id)
    {
        return new TransactionResponse();
    }
}

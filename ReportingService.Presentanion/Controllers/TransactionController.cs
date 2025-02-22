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
        var transactions = await transactionService.SearchTransaction(customerId, request);

        var response = mapper.Map<List<TransactionResponse>>(transactions);
            
        return response;
    }

    [HttpGet("{id}/customer")]
    public async Task<ActionResult<CustomerResponse>> GetCustomerByTransactionIdAsync([FromRoute] Guid id)
    {
        return Ok(new CustomerResponse());
    }
}

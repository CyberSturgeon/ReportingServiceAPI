using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportingService.Application.Models;
using ReportingService.Application.Services;
using ReportingService.Application.Services.Interfaces;
using ReportingService.Core.Configuration;
using ReportingService.Core.Configuration.Filters;
using ReportingService.Presentanion.Models;

namespace ReportingService.Presentanion.Controllers;

[Route("api/transactions")]
public class TransactionController(
    ITransactionService transactionService,
    IMapper mapper) : Controller
{
    [HttpPost("by-customer")]
    public async Task<List<TransactionResponse>> SearchTransactionsAsync(
        [FromQuery] Guid customerId, 
        [FromBody] TransactionSearchFilter request)
    {
        var transactions = await transactionService.SearchTransactionAsync(customerId, request);

        var response = mapper.Map<List<TransactionResponse>>(transactions);
            
        return response;
    }

    [HttpPost("by-account")]
    public async Task<List<TransactionResponse>> SearchTransactionsByAccountAsync(
        [FromQuery] Guid accountId)
    {
        var transactions = await transactionService.SearchTransactionByAccountAsync(accountId);

        var response = mapper.Map<List<TransactionResponse>>(transactions);

        return response;
    }

    [HttpGet("by-period")]
    public async Task<List<TransactionResponse>> GetTransactionsByPeriodAsync(
        [FromQuery] DateTimeFilter dates)
    {
        var transactions = await transactionService.GetTransactionsByPeriodAsync(dates);
        var response = mapper.Map<List<TransactionResponse>>(transactions);

        return response;
    }
}

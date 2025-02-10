
using ReportingService.Core.Configuration;

namespace ReportingService.Application.Models;

public class TransactionModel
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public decimal AmountRUB { get; set; }
    public TransactionType TransactionType { get; set; }
    //DENORMALIZED
    public Currency Currency { get; set; }
    public Guid CustomerId { get; set; }
}

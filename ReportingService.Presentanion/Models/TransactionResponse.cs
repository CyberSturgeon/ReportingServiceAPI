using ReportingService.Core.Configuration;

namespace ReportingService.Presentanion.Models;

public class TransactionResponse
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public TransactionType TransactionType { get; set; }
    public Currency Currency { get; set; }
    public Guid CustomerId { get; set; }
}

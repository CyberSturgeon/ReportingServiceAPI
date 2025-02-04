
namespace ReportingService.Application.Models;

public class ComissionModel
{
    public Guid Id { get; set; }
    public Guid TransactionId { get; set; }
    public TransactionModel Transaction { get; set; }
    public decimal Income { get; set; }
}

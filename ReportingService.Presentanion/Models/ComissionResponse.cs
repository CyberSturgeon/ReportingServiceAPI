namespace ReportingService.Presentanion.Models;

public class ComissionResponse
{
    public Guid Id { get; set; }
    public Guid TransactionId { get; set; }
    //public Transaction Transaction { get; set; }
    public decimal Income { get; set; }
}

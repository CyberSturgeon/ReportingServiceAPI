
namespace ReportingService.Persistence.Entities;

public class Comission
{
    public Guid Id {  get; set; }
    public Transaction Transaction { get; set; }
    public Guid TransactionId { get; set; }
    public decimal Income { get; set; }
    public DateTime TransactionDate { get; set; }
}

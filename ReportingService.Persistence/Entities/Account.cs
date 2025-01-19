
using ReportingService.Core.Configuration;

namespace ReportingService.Persistence.Entities;

public class Account
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime DateCreated { get; set; }
    public bool Status { get; set; }
    public Currency Currency { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = [];
    //DENORMALIZED
    public decimal AmountWithoutDrawalRUB { get; set; }
    public decimal AmountWithDrawalRUB { get; set; }
    public Customer Customer { get; set; }
}

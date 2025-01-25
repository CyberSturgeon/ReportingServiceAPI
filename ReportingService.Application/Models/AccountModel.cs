
using ReportingService.Core.Configuration;


namespace ReportingService.Application.Models;

public class AccountModel
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime DateCreated { get; set; }
    public bool Status { get; set; }
    public Currency Currency { get; set; }
    public ICollection<TransactionModel> Transactions { get; set; } = [];
    public required CustomerModel Customer { get; set; }
}

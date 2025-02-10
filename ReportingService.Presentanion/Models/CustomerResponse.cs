using ReportingService.Core.Configuration;

namespace ReportingService.Presentanion.Models;

public class CustomerResponse
{
    public Guid Id { get; set; }
    public Role Role { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Login { get; set; }
    //public string Password { get; set; }
    public DateTime BirthDate { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    //public ICollection<AccountModel> Accounts { get; set; } = [];
    public bool IsDeactivated { get; set; }
    // DENORMALIZED
    //public ICollection<TransactionModel> Transactions { get; set; } = [];
}

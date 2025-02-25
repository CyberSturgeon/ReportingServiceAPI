﻿
using ReportingService.Core.Configuration;

namespace ReportingService.Application.Models;

public class CustomerModel
{
    public Guid Id { get; set; }
    public Role Role { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public DateTime? CustomVipDueDate { get; set; }
    public string Password { get; set; }
    public DateTime BirthDate { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsDeactivated { get; set; }
    public Guid CustomerServiceId { get; set; }
    public List<AccountModel> Accounts { get; set; }
    public List<TransactionModel> Transactions { get; set; }
}

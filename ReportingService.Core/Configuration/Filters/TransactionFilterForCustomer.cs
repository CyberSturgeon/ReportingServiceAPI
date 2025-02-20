namespace ReportingService.Core.Configuration.Filters;

public class TransactionFilterForCustomer
{
    public required int TransactionsCount { get; set; }
    public required DateTime DateStart { get; set; }
    public required DateTime DateEnd { get; set; }
}

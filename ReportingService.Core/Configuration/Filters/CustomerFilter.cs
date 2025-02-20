namespace ReportingService.Core.Configuration.Filters;

public class CustomerFilter
{
    public TransactionFilterForCustomer? TransactionFilter { get; set; }
    public AccountFilter? AccountFilter { get; set; }
    public DateFilter? BdayFilter { get; set; }
}

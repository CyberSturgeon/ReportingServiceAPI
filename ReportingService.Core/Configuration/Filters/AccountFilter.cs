namespace ReportingService.Core.Configuration.Filters;

public class AccountFilter
{
    public required int? AccountsCount { get; set; }
    public required List<Currency>? Currencies { get; set; }
}

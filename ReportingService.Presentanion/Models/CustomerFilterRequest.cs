using MYPBackendMicroserviceIntegrations.Enums;

namespace ReportingService.Presentanion.Models;

public class CustomerFilterRequest
{
    public int? transactionsCount { get; set; }
    public int? accountsCount { get; set; }
    public DateTime? dateStart { get; set; }
    public DateTime? dateEnd { get; set; }
    public List<Currency>? currencies { get; set; }
    public DateTime? birth { get; set; }
}

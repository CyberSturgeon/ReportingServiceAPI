using MYPBackendMicroserviceIntegrations.Enums;

namespace ReportingService.Presentanion.Models
{
    public class AccountResponse
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeactivated { get; set; }
        public Currency Currency { get; set; }
    }
}

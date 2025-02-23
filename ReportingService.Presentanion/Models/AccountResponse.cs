using ReportingService.Core.Configuration;
using ReportingService.Persistence.Entities;

namespace ReportingService.Presentanion.Models
{
    public class AccountResponse
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Status { get; set; }
        public Currency Currency { get; set; }
    }
}

using ReportingService.Application.Models;
using ReportingService.Core.Configuration.Filters;

namespace ReportingService.Application.Services.Interfaces
{
    public interface IComissionService
    {
        Task<ComissionModel> AddComissionAsync(ComissionModel comissionModel);
        Task<ComissionModel> GetComissionByIdAsync(Guid id);
        Task<ComissionModel> GetComissionByTransactionIdAsync(Guid transactionId);
        Task<IEnumerable<ComissionModel>> GetComissionsAsync(Guid? customerId, Guid? accountId, DateTimeFilter date);
        Task TransactionalAddComissionsAsync(List<ComissionModel> comissionModels);
    }
}
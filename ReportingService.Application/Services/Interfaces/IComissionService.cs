using ReportingService.Application.Models;
using ReportingService.Core.Configuration.Filters;

namespace ReportingService.Application.Services.Interfaces
{
    public interface IComissionService
    {
        Task<ComissionModel> AddAsync(ComissionModel comissionModel);
        Task<ComissionModel> GetByIdAsync(Guid id);
        Task<ComissionModel> GetComissionByTransactionIdAsync(Guid transactionId);
        Task<IEnumerable<ComissionModel>> GetAsync(Guid? customerId, Guid? accountId, DateTimeFilter date);
        Task TransactionalAddAsync(List<ComissionModel> comissionModels);
    }
}
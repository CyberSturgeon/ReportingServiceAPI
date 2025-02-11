using ReportingService.Application.Models;

namespace ReportingService.Application.Services.Interfaces
{
    public interface IComissionService
    {
        Task<ComissionModel> AddComissionAsync(ComissionModel comissionModel);
        Task<ComissionModel> GetComissionByIdAsync(Guid id);
        Task<ComissionModel> GetComissionByTransactionIdAsync(Guid transactionId);
        Task<IEnumerable<ComissionModel>> GetComissionsAsync(Guid? customerId, Guid? accountId, DateTime? dateStart, DateTime? dateEnd);
        Task TransactionalAddComissionsAsync(List<ComissionModel> comissionModels);
    }
}
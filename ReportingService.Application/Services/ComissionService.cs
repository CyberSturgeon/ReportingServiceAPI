
using AutoMapper;
using ReportingService.Application.Exceptions;
using ReportingService.Application.Models;
using ReportingService.Application.Services.Interfaces;
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories.Interfaces;

namespace ReportingService.Application.Services;

public class ComissionService(
        ITransactionRepository transactionRepository,
        IComissionRepository comissionRepository,
        IMapper mapper) : IComissionService
{
    public async Task<ComissionModel> GetComissionByIdAsync(Guid id)
    {
        var comission = await comissionRepository.GetByIdAsync(id) ??
            throw new EntityNotFoundException($"Comission {id} not found");
        var comissionModel = mapper.Map<ComissionModel>(comission);

        return comissionModel;
    }

    public async Task<ComissionModel> AddComissionAsync(ComissionModel comissionModel)
    {
        var transaction = await transactionRepository.GetByIdAsync(comissionModel.TransactionId) ??
            throw new EntityNotFoundException($"Transaction {comissionModel.TransactionId} related to comission not found");

        var comission = await comissionRepository.AddAndReturnAsync(mapper.Map<Comission>(comissionModel));

        return mapper.Map<ComissionModel>(comission);
    }

    public async Task TransactionalAddComissionsAsync(List<ComissionModel> comissionModels)
    {
        var transactionIds = comissionModels.Select(cm => cm.TransactionId).Distinct().ToList();

        var transactionModels = mapper.Map<List<TransactionModel>>(await transactionRepository
                .FindManyAsync(x => transactionIds.Contains(x.Id))).ToList();

        if (!transactionModels.Any())
        {
            throw new EntityNotFoundException("No one transaction related to comissions");
        }

        transactionIds = transactionModels.Select(x => x.Id).Distinct().ToList();
        comissionModels.RemoveAll(x => !transactionIds.Contains(x.TransactionId));

        await comissionRepository.TransactionalAddRangeAsync(mapper.Map<List<Comission>>(comissionModels));
    }

    public async Task<ComissionModel> GetComissionByTransactionIdAsync(Guid transactionId)
    {
        var transaction = await transactionRepository.GetByIdAsync(transactionId) ??
            throw new EntityNotFoundException($"Transaction {transactionId} not found");

        var comission = await comissionRepository.FindAsync(
            x => x.Transaction.Id == transactionId) ??
                throw new EntityNotFoundException($"Comssion with transaction {transactionId} not found");

        var comissionModel = mapper.Map<ComissionModel>(comission);

        return comissionModel;
    }

    public async Task<IEnumerable<ComissionModel>> GetComissionsAsync(Guid? customerId, Guid? accountId,
                    DateTime? dateStart, DateTime? dateEnd)
    {
        var commisions = await comissionRepository.FindManyAsync(x =>
            customerId == null || x.Transaction.CustomerId == customerId &&
            accountId == null || x.Transaction.AccountId == accountId &&
            dateStart == null || x.Transaction.Date >= dateStart &&
            dateEnd == null || x.Transaction.Date <= dateEnd);

        var comissionModels = mapper.Map<List<ComissionModel>>(commisions.ToList());

        return comissionModels; //НУЖНА ЛИ ПРОВЕРКА НА СУЩЕСТВОВАНИЕ АККАУНТА И КАСТОМЕРА?
    }
}

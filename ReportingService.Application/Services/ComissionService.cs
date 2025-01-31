
using AutoMapper;
using ReportingService.Application.Exceptions;
using ReportingService.Application.Models;
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories.Interfaces;

namespace ReportingService.Application.Services;

public class ComissionService(
        ITransactionRepository transactionRepository,
        IComissionRepository comissionRepository,
        IMapper mapper)
{
    public async Task<ComissionModel> GetComissionByIdAsync(Guid id)
    {
        var comission = await comissionRepository.GetByIdAsync(id) ??
            throw new EntityNotFoundException($"Comission {id} not found");
        var comissionModel = mapper.Map<ComissionModel>(comission);

        return comissionModel;
    }

    public async Task AddComissionAsync(ComissionModel comissionModel)
    {
        var transaction = await transactionRepository.GetByIdAsync(comissionModel.TransactionId) ??
            throw new EntityNotFoundException($"Transaction {comissionModel.TransactionId} related to comission not found");

        await comissionRepository.AddAsync(mapper.Map<Comission>(comissionModel));
    }

    public async Task TransactionalAddComissionsAsync(List<ComissionModel> comissionModels)
    {
        var transactionIds = comissionModels.Select(cm => cm.TransactionId).Distinct().ToList();

        var transactionModels = mapper.Map<List<TransactionModel>>(await transactionRepository
                .FindManyAsync(x => transactionIds.Contains(x.Id)));

        if (!transactionModels.Any())
        {
            throw new EntityNotFoundException("No one transaction related to comissions");
        }

        foreach (var comission in comissionModels)
        {
            if (!transactionModels.Contains(comission.Transaction))
            {
                comissionModels.Remove(comission);
            }
        }

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
            dateEnd == null || x.Transaction.Date<=dateEnd);

        var comissionModels = mapper.Map<List<ComissionModel>>(commisions.ToList());

        return comissionModels; //НУЖНА ЛИ ПРОВЕРКА НА СУЩЕСТВОВАНИЕ АККАУНТА И КАСТОМЕРА?
    }
}

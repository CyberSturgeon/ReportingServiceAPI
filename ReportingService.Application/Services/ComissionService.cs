using AutoMapper;
using Microsoft.Extensions.Logging;
using ReportingService.Application.Exceptions;
using ReportingService.Application.Models;
using ReportingService.Application.Services.Interfaces;
using ReportingService.Core.Configuration.Filters;
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories.Interfaces;

namespace ReportingService.Application.Services;

public class ComissionService(
        ITransactionRepository transactionRepository,
        IComissionRepository comissionRepository,
        IMapper mapper, ILogger<ComissionService> logger) : IComissionService
{
    public async Task<ComissionModel> GetComissionByIdAsync(Guid id)
    {
        logger.LogInformation($"GET comission {id}");
        var comission = await comissionRepository.GetByIdAsync(id) ??
            throw new EntityNotFoundException($"Comission {id} not found");
        var comissionModel = mapper.Map<ComissionModel>(comission);
        logger.LogInformation("SUCESS");
        return comissionModel;
    }

    public async Task<ComissionModel> AddComissionAsync(ComissionModel comissionModel)
    {
        logger.LogInformation($"CREATE comission for transaction {comissionModel.TransactionId}");
        var transaction = await transactionRepository.GetByIdAsync(comissionModel.TransactionId) ??
            throw new EntityNotFoundException($"Transaction {comissionModel.TransactionId} related to comission not found");

        var comission = await comissionRepository.AddAndReturnAsync(mapper.Map<Comission>(comissionModel));
        logger.LogInformation("SUCESS");
        return mapper.Map<ComissionModel>(comission);
    }

    public async Task TransactionalAddComissionsAsync(List<ComissionModel> comissionModels)
    {
        logger.LogInformation($"CREATE {comissionModels.Count} comissions");
        var transactionIds = comissionModels.Select(cm => cm.TransactionId).Distinct().ToList();

        var transactionModels = mapper.Map<List<TransactionModel>>(await transactionRepository
                .FindManyAsync(x => transactionIds.Contains(x.Id))).ToList();

        if (!transactionModels.Any())
        {
            throw new EntityNotFoundException("No one transaction related to comissions");
        }

        transactionIds = transactionModels.Select(x => x.Id).Distinct().ToList();
        comissionModels.RemoveAll(x => !transactionIds.Contains(x.TransactionId));

        logger.LogInformation($"After transaction id check CREATE {comissionModels.Count} comissions");
        await comissionRepository.TransactionalAddRangeAsync(mapper.Map<List<Comission>>(comissionModels));
        logger.LogInformation("SUCESS");
    }

    public async Task<ComissionModel> GetComissionByTransactionIdAsync(Guid transactionId)
    {
        logger.LogInformation($"GET comission for transaction {transactionId}");
        var transaction = await transactionRepository.GetByIdAsync(transactionId) ??
            throw new EntityNotFoundException($"Transaction {transactionId} not found");

        var comission = await comissionRepository.FindAsync(
            x => x.Transaction.Id == transactionId) ??
                throw new EntityNotFoundException($"Comssion with transaction {transactionId} not found");

        var comissionModel = mapper.Map<ComissionModel>(comission);
        logger.LogInformation("SUCESS");
        return comissionModel;
    }

    public async Task<IEnumerable<ComissionModel>> GetComissionsAsync(Guid? customerId, Guid? accountId,
                    DateFilter date)
    {
        logger.LogInformation($"GET comissions by filter: customer {customerId}, account {accountId}, dates {date.DateStart} - {date.DateEnd}");
        var commisions = await comissionRepository.FindManyAsync(x =>
            customerId == null || x.Transaction.CustomerId == customerId &&
            accountId == null || x.Transaction.AccountId == accountId &&
            date == null || x.Transaction.Date>= date.DateStart && x.Transaction.Date < date.DateEnd);

        var comissionModels = mapper.Map<List<ComissionModel>>(commisions.ToList());
        logger.LogInformation("SUCESS");
        return comissionModels;
    }
}

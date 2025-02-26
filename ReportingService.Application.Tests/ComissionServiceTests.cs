using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ReportingService.Application.Exceptions;
using ReportingService.Application.Mappings;
using ReportingService.Application.Models;
using ReportingService.Application.Services;
using ReportingService.Application.Tests.TestCases;
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories.Interfaces;
using System.Linq.Expressions;

namespace ReportingService.Application.Tests;

public class ComissionServiceTests
{
    private readonly Mock<IComissionRepository> _comissionRepositoryMock;
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mapper _mapper;
    private readonly ComissionService _sut;
    private readonly Mock<ILogger<ComissionService>> _comissionServiceLoggerMock;

    public ComissionServiceTests()
    {
        _comissionRepositoryMock = new();
        _transactionRepositoryMock = new();
        _mapper = new(MapperHelper.ConfigureMapper());
        _comissionServiceLoggerMock = new();
        _sut = new(_transactionRepositoryMock.Object, _comissionRepositoryMock.Object,
            _mapper, _comissionServiceLoggerMock.Object);
    }

    [Fact]
    public async Task GetComissionByIdAsync_NonExistsComission_EntityNotFoundExceptionThrown()
    {
        var id = Guid.NewGuid();
        var msg = $"Comission {id} not found";

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetComissionByIdAsync(id));

        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetComissionByIdAsync_ExistsComission_GetSucess()
    {   
        var comission = ComissionTestCase.GetComissionEntity(null, null);

        _comissionRepositoryMock.Setup(x => x.GetByIdAsync(comission.Id)).ReturnsAsync(comission);
        var comissionModel = _mapper.Map<ComissionModel>(comission);

        var comissionResponse = await _sut.GetComissionByIdAsync(comission.Id);

        _comissionRepositoryMock.Verify(x => x.GetByIdAsync(comission.Id), Times.Once);
        Assert.Equivalent(comissionModel, comissionResponse);
    }

    [Fact]
    public async Task GetComissionByTransactionIdAsync_NonExistsTransaction_EntityNotFoundExceptionThrown()
    {
        var id = Guid.NewGuid();
        var msg = ($"Transaction {id} not found");

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetComissionByTransactionIdAsync(id));

        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetComissionByTransactionIdAsync_ExistsTransactionNonExistsComission_EntityNotFoundExceptionThrown()
    {
        var transaction = TransactionTestCase.GetTransactionEntity(null, null);
        var msg = ($"Comssion with transaction {transaction.Id} not found");
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(transaction.Id)).ReturnsAsync(transaction);

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetComissionByTransactionIdAsync(transaction.Id));

        _transactionRepositoryMock.Verify(x => x.GetByIdAsync(transaction.Id), Times.Once);
        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetComissionByTransactionIdAsync_ExistsTransactionExistsComission_GetSucess()
    {
        var transaction = TransactionTestCase.GetTransactionEntity(null, null);
        var comission = ComissionTestCase.GetComissionEntity(null, null);
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(transaction.Id)).ReturnsAsync(transaction);
        _comissionRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Comission, bool>>>(),
             null))
            .ReturnsAsync(comission);
        var comissionModel = _mapper.Map<ComissionModel>(comission);

        var comissionResponse = await _sut.GetComissionByTransactionIdAsync(transaction.Id);

        _transactionRepositoryMock.Verify(x => x.GetByIdAsync(transaction.Id), Times.Once);
        _comissionRepositoryMock.Verify(x => x.FindAsync(It.IsAny<Expression<Func<Comission, bool>>>(),
            null), Times.Once);
        Assert.Equivalent(comissionModel, comissionResponse);
    }

    [Fact]
    public async Task AddComissionAsync_NonExistsTransaction_EntityNotFoundExceptionThrown()
    {
        var id = Guid.NewGuid();
        var comissionModel = ComissionTestCase.GetComissionModel(id, null);
        var msg = $"Transaction {id} related to comission not found";

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.AddComissionAsync(comissionModel));

        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task AddComissionAsync_ExistsTransaction_AddSucess()
    {
        var transaction = TransactionTestCase.GetTransactionEntity(null, null);
        var comissionModel = ComissionTestCase.GetComissionModel(transaction.Id, null);
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(transaction.Id)).ReturnsAsync(transaction);
        _comissionRepositoryMock.Setup(x =>
            x.AddAndReturnAsync(It.Is<Comission>(x =>
                x.Id == comissionModel.Id && x.TransactionId == transaction.Id))).ReturnsAsync(_mapper.Map<Comission>(comissionModel));

        var comissionResponse = await _sut.AddComissionAsync(comissionModel);

        _transactionRepositoryMock.Verify(x => x.GetByIdAsync(transaction.Id), Times.Once);
        _comissionRepositoryMock.Verify(x =>
            x.AddAndReturnAsync(It.Is<Comission>(x => x.Id == comissionModel.Id &&
            x.TransactionId == comissionModel.TransactionId)), Times.Once);
        Assert.Equivalent(comissionModel, comissionResponse);
    }

    [Fact]
    public async Task TransactionalAddComissionsAsync_NoOneTransactionsRelatedToComissions_EntityNotFoundExceptionTrown()
    {
        var comissionModels = new List<ComissionModel> { ComissionTestCase.GetComissionModel(null, null),
                                                         ComissionTestCase.GetComissionModel(null, null),
                                                         ComissionTestCase.GetComissionModel(null, null)};

        var msg = "No one transaction related to comissions";

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.TransactionalAddComissionsAsync(comissionModels));

        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task TransactionalAddComissionsAsync_OneComissionNoRelatedToTransaction_SucessAddTwoComissions()
    {
        var transactions = new List<Transaction> {
            TransactionTestCase.GetTransactionEntity(null, null),
            TransactionTestCase.GetTransactionEntity(null, null)};

        var comissionModels = new List<ComissionModel> {
            ComissionTestCase.GetComissionModel(transactions[0].Id, _mapper.Map<TransactionModel>(transactions[0])),
            ComissionTestCase.GetComissionModel(transactions[1].Id, _mapper.Map<TransactionModel>(transactions[1])),
            ComissionTestCase.GetComissionModel(null, null)};

        _transactionRepositoryMock.Setup(x => x.FindManyAsync(It.IsAny<Expression<Func<Transaction, bool>>>()))
            .ReturnsAsync(transactions);

        await _sut.TransactionalAddComissionsAsync(comissionModels);

        _comissionRepositoryMock.Verify(x =>
            x.TransactionalAddRangeAsync(It.Is<List<Comission>>(x => x.Count == 2 &&
                x[0].TransactionId == transactions[0].Id && x[1].TransactionId == transactions[1].Id)), Times.Once);
    }
}

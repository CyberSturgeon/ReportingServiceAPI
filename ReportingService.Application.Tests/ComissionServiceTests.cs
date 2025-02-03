using AutoMapper;
using Moq;
using ReportingService.Application.Exceptions;
using ReportingService.Application.Mappings;
using ReportingService.Application.Models;
using ReportingService.Application.Services;
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

    public ComissionServiceTests()
    {
        _comissionRepositoryMock = new();
        _transactionRepositoryMock = new();
        _mapper = new(MapperHelper.ConfigureMapper());

        _sut = new(_transactionRepositoryMock.Object, _comissionRepositoryMock.Object, _mapper);
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
        var id = Guid.NewGuid();
        
        var comission = new Comission { Id = id };
        _comissionRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(comission);
        var comissionModel = _mapper.Map<ComissionModel>(comission);

        var comissionResponse = await _sut.GetComissionByIdAsync(id);

        _comissionRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
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
        var id = Guid.NewGuid();
        var transaction = new Transaction { Id = id };
        var msg = ($"Comssion with transaction {id} not found");
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(transaction);

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetComissionByTransactionIdAsync(id));

        _transactionRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetComissionByTransactionIdAsync_ExistsTransactionExistsComission_GetSucess()
    {
        var id = Guid.NewGuid();
        var transaction = new Transaction { Id = id };
        var comission = new Comission { Id = id };
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(transaction);
        _comissionRepositoryMock.Setup(x => x.FindAsync(x => x.Transaction.Id == id))
            .ReturnsAsync(comission);
        var comissionModel = _mapper.Map<ComissionModel>(comission);

        var comissionResponse = await _sut.GetComissionByTransactionIdAsync(id);

        _transactionRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        _comissionRepositoryMock.Verify(x => x.FindAsync(x => x.Transaction.Id == id), Times.Once);
        Assert.Equivalent(comissionModel, comissionResponse);
    }

    [Fact]
    public async Task AddComissionAsync_NonExistsTransaction_EntityNotFoundExceptionThrown()
    {
        var id = Guid.NewGuid();
        var comissionModel = new ComissionModel { TransactionId = id };
        var msg = $"Transaction {id} related to comission not found";

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.AddComissionAsync(comissionModel));

        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task AddComissionAsync_ExistsTransaction_AddSucess()
    {
        var id = Guid.NewGuid();
        var comissionModel = new ComissionModel {Id = id, TransactionId = id};
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(new Transaction { Id = id });
        _comissionRepositoryMock.Setup(x =>
            x.AddAndReturnAsync(It.Is<Comission>(x =>
                x.Id == id && x.TransactionId == id))).ReturnsAsync(_mapper.Map<Comission>(comissionModel));

        var comissionResponse = await _sut.AddComissionAsync(comissionModel);

        _transactionRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        _comissionRepositoryMock.Verify(x =>
            x.AddAndReturnAsync(It.Is<Comission>(x => x.Id == comissionModel.Id &&
            x.TransactionId == comissionModel.TransactionId)), Times.Once);
        Assert.Equivalent(comissionModel, comissionResponse);
    }

    [Fact]
    public async Task TransactionalAddComissionsAsync_NoOneTransactionsRelatedToComissions_EntityNotFoundExceptionTrown()
    {
        var comissionModels = new List<ComissionModel> { new ComissionModel { TransactionId = Guid.NewGuid()},
                                                         new ComissionModel { TransactionId = Guid.NewGuid()},
                                                         new ComissionModel { TransactionId = Guid.NewGuid()}};

        var msg = "No one transaction related to comissions";

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.TransactionalAddComissionsAsync(comissionModels));

        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task TransactionalAddComissionsAsync_OneComissionNoRelatedToTransaction_SucessAddTwoComissions()
    {
        var transactions = new List<Transaction> {
            new Transaction { Id = Guid.NewGuid() },
            new Transaction { Id = Guid.NewGuid() }};

        var comissionModels = new List<ComissionModel> {
            new ComissionModel { TransactionId = transactions[0].Id, Transaction = _mapper.Map<TransactionModel>(transactions[0]) },
            new ComissionModel { TransactionId = transactions[1].Id, Transaction = _mapper.Map<TransactionModel>(transactions[1])},
            new ComissionModel()};

        _transactionRepositoryMock.Setup(x => x.FindManyAsync(It.IsAny<Expression<Func<Transaction, bool>>>()))
            .ReturnsAsync(transactions); // Не работает x2

        await _sut.TransactionalAddComissionsAsync(comissionModels);

        _comissionRepositoryMock.Verify(x =>
            x.TransactionalAddRangeAsync(It.Is<List<Comission>>(x => x.Count == 2 &&
                x[0].TransactionId == transactions[0].Id && x[1].TransactionId == transactions[1].Id)), Times.Once);
    }
}

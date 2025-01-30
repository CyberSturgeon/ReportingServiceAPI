using AutoMapper;
using Moq;
using ReportingService.Application.Exceptions;
using ReportingService.Application.Mappings;
using ReportingService.Application.Models;
using ReportingService.Application.Services;
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories.Interfaces;

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

        var config = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile(new CustomerMapperProfile());
                cfg.AddProfile(new AccountMapperProfile());
                cfg.AddProfile(new TransactionMapperProfile());
                cfg.AddProfile(new ComissionMapperProfile());
            });
        _mapper = new(config);

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
        _comissionRepositoryMock.Setup(x => x.GetByIdAsync(id).Result).Returns(comission);
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
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(id).Result).Returns(transaction);

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
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(id).Result).Returns(transaction);
        _comissionRepositoryMock.Setup(x => x.FindAsync(x => x.Transaction.Id == id).Result)
            .Returns(new List<Comission> { comission });
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
        var comission = _mapper.Map<Comission>(comissionModel);
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(id).Result).Returns(new Transaction { Id = id });

        await _sut.AddComissionAsync(comissionModel);

        _transactionRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        _comissionRepositoryMock.Verify(x => x.AddAsync(comission), Times.Once);
    }

    //[Fact]
    //public async Task 
}

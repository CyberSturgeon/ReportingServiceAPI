
using AutoMapper;
using Moq;
using ReportingService.Application.Exceptions;
using ReportingService.Application.Mappings;
using ReportingService.Application.Models;
using ReportingService.Application.Services;
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories.Interfaces;

namespace ReportingService.Application.Tests;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mapper _mapper;
    private readonly CustomerService _sut;

    public CustomerServiceTests()
    {
        _customerRepositoryMock = new();
        _accountRepositoryMock = new();
        _transactionRepositoryMock = new();

        _mapper.ConfigureMapper();

        _sut = new(_customerRepositoryMock.Object, _transactionRepositoryMock.Object, _accountRepositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_NonExistingUser_EntityNotFoundExceptionThrown()
    {
        var id = Guid.NewGuid();
        var msg = $"Customer {id} not found";

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetCustomerByIdAsync(id));

        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ExistingUser_GetSucess()
    {
        var id = Guid.NewGuid();
        var customer = new Customer { Id = id };
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(customer);
        var customerModel = _mapper.Map<CustomerModel>(customer);

        var customerResponse = await _sut.GetCustomerByIdAsync(id);

        _customerRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        Assert.Equivalent(customerModel, customerResponse);
    }

    [Fact]
    public async Task GetFullCustomerByIdAsync_NonExistingUser_EntityNotFoundExceptionThrown()
    {
        var id = Guid.NewGuid();
        var msg = $"Customer {id} not found";

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetFullCustomerByIdAsync(id));

        _customerRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetFullCustomerByIdAsync_ExistingUserNoAccounts_EntityNotFoundExceptionThrown()
    {
        var id = Guid.NewGuid();
        var msg = $"Customer {id} have no accounts";
        var customer = new Customer { Id = id };
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(customer);
        var customerModel = _mapper.Map<CustomerModel>(customer);

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetFullCustomerByIdAsync(id));

        _customerRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        _accountRepositoryMock.Verify(x => x.FindManyAsync(x => x.CustomerId == id), Times.Once);
        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetFullCustomerByIdAsync_ExistingUserHaveAccountsNoTransactions_GetSucess()
    {
        var id = Guid.NewGuid();
        var msg = $"Customer {id} have no accounts";
        var customer = new Customer { Id = id };
        var accounts = new List<Account> { new Account { Id = id } };
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(customer);
        _accountRepositoryMock.Setup(x => x.FindManyAsync(x => x.CustomerId == id)).ReturnsAsync(accounts);   
        var customerModel = _mapper.Map<CustomerModel>(customer);
        var accountModels = _mapper.Map<List<AccountModel>>(accounts);
        customerModel.Accounts = accountModels;

        var customerResponse = await _sut.GetFullCustomerByIdAsync(id);

        _customerRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        _accountRepositoryMock.Verify(x => x.FindManyAsync(x => x.CustomerId == id), Times.Once);
        _transactionRepositoryMock.Verify(x => x.FindManyAsync(x => x.CustomerId == id), Times.Once);
        Assert.Equivalent(customerModel, customerResponse);
    }

    [Fact]
    public async Task GetFullCustomerByIdAsync_ExistingUserHaveAccountsHaveTransactions_GetSucess()
    {
        var id = Guid.NewGuid();
        var customer = new Customer { Id = id };
        var accounts = new List<Account> { new Account { Id = id } };
        var transactions = new List<Transaction> { new Transaction { Id = id } };
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(customer);
        _accountRepositoryMock.Setup(x => x.FindManyAsync(x => x.CustomerId == id)).ReturnsAsync(accounts);
        _transactionRepositoryMock.Setup(x => x.FindManyAsync(x => x.CustomerId == id)).ReturnsAsync(transactions);
        var customerModel = _mapper.Map<CustomerModel>(customer);
        var accountModels = _mapper.Map<List<AccountModel>>(accounts);
        var transactionModels = _mapper.Map<List<TransactionModel>>(transactions);  
        customerModel.Accounts = accountModels;
        customerModel.Transactions = transactionModels;

        var customerResponse = await _sut.GetFullCustomerByIdAsync(id);

        _customerRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        _accountRepositoryMock.Verify(x => x.FindManyAsync(x => x.CustomerId == id), Times.Once);
        _transactionRepositoryMock.Verify(x => x.FindManyAsync(x => x.CustomerId == id), Times.Once);
        Assert.Equivalent(customerModel, customerResponse);
    }

    [Fact]
    public async Task GetCustomerByAccountIdAsync_NonExistsAccount_EntityNotFoundExceptionThrown()
    {
        var id = Guid.NewGuid();
        var msg = $"Account {id} not found";

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetCustomerByAccountIdAsync(id));

        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetCustomerByAccountIdAsync_ExistsAccountNonExistsCustomer_EntityNotFoundExceptionThrown()
    {
        var id = Guid.NewGuid();
        var msg = $"Customer with account {id} not found";
        var account = new Account { CustomerId = id };
        _accountRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(account);

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetCustomerByAccountIdAsync(id));

        _accountRepositoryMock.Verify(x=> x.GetByIdAsync(id), Times.Once);
        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetCustomerByAccountIdAsync_ExistsAccountExistsCustomer_GetSucess()
    {
        var id = Guid.NewGuid();
        var account = new Account { CustomerId = id };
        var customer = new Customer { Id = id };    
        _accountRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(account);
        _customerRepositoryMock.Setup(x => x.FindManyAsync(x => x.Accounts.Contains(account)))
            .ReturnsAsync(new List<Customer> { customer });
        var customerModel = _mapper.Map<CustomerModel>(customer);
        
        var customerResponse = await _sut.GetCustomerByAccountIdAsync(id);

        _accountRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        _customerRepositoryMock.Verify(x => x.FindManyAsync(x => x.Accounts.Contains(account)), Times.Once);
        Assert.Equivalent(customerModel, customerResponse);
    }

    [Fact]
    public async Task GetCustomerByTransactionIdAsync_NonExistsTransaction_EntityNotFoundExceptionThrown()
    {
        var id = Guid.NewGuid();
        var msg = $"Transaction {id} not found";

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetCustomerByTransactionIdAsync(id));

        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetCustomerByTransactionIdAsync_ExistsTransactionNonExistsCustomer_EntityNotFoundExceptionThrown()
    {
        var id = Guid.NewGuid();
        var transaction = new Transaction { Id = id };
        var msg = $"Customer with transaction {id} not found";
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(transaction);

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetCustomerByTransactionIdAsync(id));

        _transactionRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        Assert.Equal(msg , ex.Message);
    }

    [Fact]
    public async Task GetCustomerByTransactionIdAsync_ExistsTransactionExistsCustomer_GetSucess()
    {
        var id = Guid.NewGuid();
        var transaction = new Transaction { Id = id };
        var customer = new Customer { Id = id };
        _customerRepositoryMock.Setup(x => x.FindManyAsync(x => x.Transactions.Contains(transaction)))
            .ReturnsAsync(new List<Customer> { customer });
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(transaction);
        var customerModel = _mapper.Map<CustomerModel>(customer);

        var customerResponse = await _sut.GetCustomerByTransactionIdAsync(id);

        _transactionRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        _customerRepositoryMock.Verify(x => x.FindManyAsync(x => x.Transactions.Contains(transaction)), Times.Once);
        Assert.Equivalent(customerModel, customerResponse);
    }
}

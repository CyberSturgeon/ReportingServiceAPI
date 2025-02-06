
using AutoMapper;
using Moq;
using ReportingService.Core.Configuration;
using ReportingService.Application.Exceptions;
using ReportingService.Application.Mappings;
using ReportingService.Application.Models;
using ReportingService.Application.Services;
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories.Interfaces;
using ReportingService.Application.Tests.TestCases;
using System.Linq.Expressions;

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

        _mapper = new(MapperHelper.ConfigureMapper());

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
        var customer = CustomerTestCase.GetCustomerEntity(null, null);
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(customer.Id)).ReturnsAsync(customer);
        var customerModel = _mapper.Map<CustomerModel>(customer);

        var customerResponse = await _sut.GetCustomerByIdAsync(customer.Id);

        _customerRepositoryMock.Verify(x => x.GetByIdAsync(customer.Id), Times.Once);
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
        var customer = CustomerTestCase.GetCustomerEntity(null, null);
        var msg = $"Customer {customer.Id} have no accounts";
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(customer.Id)).ReturnsAsync(customer);
        var customerModel = _mapper.Map<CustomerModel>(customer);

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetFullCustomerByIdAsync(customer.Id));

        _customerRepositoryMock.Verify(x => x.GetByIdAsync(customer.Id), Times.Once);
        _accountRepositoryMock.Verify(x => x.FindManyAsync(x => x.CustomerId == customer.Id), Times.Once);
        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetFullCustomerByIdAsync_ExistingUserHaveAccountsNoTransactions_GetSucess()
    {
        var customer = CustomerTestCase.GetCustomerEntity(null, null);
        var accounts = new List<Account> { AccountTestCase.GetAccountEntity(customer.Id, null, customer)};
        customer.Accounts = accounts;
        var msg = $"Customer {customer.Id} have no accounts";
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(customer.Id)).ReturnsAsync(customer);
        _accountRepositoryMock.Setup(x => x.FindManyAsync(x => x.CustomerId == customer.Id)).ReturnsAsync(accounts);   
        var customerModel = _mapper.Map<CustomerModel>(customer);
        var accountModels = _mapper.Map<List<AccountModel>>(accounts);
        customerModel.Accounts = accountModels;

        var customerResponse = await _sut.GetFullCustomerByIdAsync(customer.Id);

        _customerRepositoryMock.Verify(x => x.GetByIdAsync(customer.Id), Times.Once);
        _accountRepositoryMock.Verify(x => x.FindManyAsync(x => x.CustomerId == customer.Id), Times.Once);
        _transactionRepositoryMock.Verify(x => x.FindManyAsync(x => x.CustomerId == customer.Id), Times.Once);
        Assert.Equivalent(customerModel, customerResponse);
    }

    [Fact]
    public async Task GetFullCustomerByIdAsync_ExistingUserHaveAccountsHaveTransactions_GetSucess()
    {
        var customer = CustomerTestCase.GetCustomerEntity(null, null);
        var accounts = new List<Account> { AccountTestCase.GetAccountEntity(customer.Id, null, customer) };
        var transactions = new List<Transaction> { TransactionTestCase.GetTransactionEntity(accounts[0].Id, customer.Id) };
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(customer.Id)).ReturnsAsync(customer);
        _accountRepositoryMock.Setup(x => x.FindManyAsync(It.IsAny<Expression<Func<Account, bool>>>())).ReturnsAsync(accounts);
        _transactionRepositoryMock.Setup(x => x.FindManyAsync(It.IsAny<Expression<Func<Transaction, bool>>>())).ReturnsAsync(transactions);
        var customerModel = _mapper.Map<CustomerModel>(customer);
        var accountModels = _mapper.Map<List<AccountModel>>(accounts);
        var transactionModels = _mapper.Map<List<TransactionModel>>(transactions);  
        customerModel.Accounts = accountModels;
        customerModel.Transactions = transactionModels;

        var customerResponse = await _sut.GetFullCustomerByIdAsync(customer.Id);

        _customerRepositoryMock.Verify(x => x.GetByIdAsync(customer.Id), Times.Once);
        _accountRepositoryMock.Verify(x => x.FindManyAsync(It.IsAny<Expression<Func<Account, bool>>>()), Times.Once);
        _transactionRepositoryMock.Verify(x => x.FindManyAsync(It.IsAny<Expression<Func<Transaction, bool>>>()), Times.Once);
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
        var account = AccountTestCase.GetAccountEntity(null, null, null);
        var msg = $"Customer with account {account.Id} not found";
        _accountRepositoryMock.Setup(x => x.GetByIdAsync(account.Id)).ReturnsAsync(account);

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetCustomerByAccountIdAsync(account.Id));

        _accountRepositoryMock.Verify(x=> x.GetByIdAsync(account.Id), Times.Once);
        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetCustomerByAccountIdAsync_ExistsAccountExistsCustomer_GetSucess()
    {
        var customer = CustomerTestCase.GetCustomerEntity(null, null);    
        var account = AccountTestCase.GetAccountEntity(customer.Id, null, customer);
        customer.Accounts.Add(account);
        _accountRepositoryMock.Setup(x => x.GetByIdAsync(account.Id)).ReturnsAsync(account);
        _customerRepositoryMock.Setup(x => x.FindAsync(x => x.Accounts.Contains(account)))
            .ReturnsAsync(customer);
        var customerModel = _mapper.Map<CustomerModel>(customer);
        
        var customerResponse = await _sut.GetCustomerByAccountIdAsync(account.Id);

        _accountRepositoryMock.Verify(x => x.GetByIdAsync(account.Id), Times.Once);
        _customerRepositoryMock.Verify(x => x.FindAsync(x => x.Accounts.Contains(account)), Times.Once);
        Assert.Equivalent(customerModel.Accounts, customerResponse.Accounts);
        //ЛИСТЫ НЕ СРАВНИВАЮТСЯ
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
        var transaction = TransactionTestCase.GetTransactionEntity(null, null);
        var msg = $"Customer with transaction {transaction.Id} not found";
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(transaction.Id)).ReturnsAsync(transaction);

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetCustomerByTransactionIdAsync(transaction.Id));

        _transactionRepositoryMock.Verify(x => x.GetByIdAsync(transaction.Id), Times.Once);
        Assert.Equal(msg , ex.Message);
    }

    [Fact]
    public async Task GetCustomerByTransactionIdAsync_ExistsTransactionExistsCustomer_GetSucess()
    {
        var customer = CustomerTestCase.GetCustomerEntity(null, null);
        var transaction = TransactionTestCase.GetTransactionEntity(null, customer.Id);
        customer.Transactions.Add(transaction);
        _customerRepositoryMock.Setup(x => x.FindAsync(x => x.Transactions.Contains(transaction)))
            .ReturnsAsync(customer);
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(transaction.Id)).ReturnsAsync(transaction);
        var customerModel = _mapper.Map<CustomerModel>(customer);

        var customerResponse = await _sut.GetCustomerByTransactionIdAsync(transaction.Id);

        _transactionRepositoryMock.Verify(x => x.GetByIdAsync(transaction.Id), Times.Once);
        _customerRepositoryMock.Verify(x => x.FindAsync(x => x.Transactions.Contains(transaction)), Times.Once);
        Assert.Equivalent(customerModel, customerResponse);
    }

    [Fact]
    public async Task AddCustomerAsync_AddSucess()
    {
        var customer = CustomerTestCase.GetCustomerEntity(null, null);
        _customerRepositoryMock.Setup(x =>
            x.AddAndReturnAsync(It.Is<Customer>(x => x.Id == customer.Id))).ReturnsAsync(customer);
        var customerModel = _mapper.Map<CustomerModel>(customer);

        var customerResponse = await _sut.AddCustomerAsync(customerModel);

        _customerRepositoryMock.Verify(x => x.AddAndReturnAsync(It.Is<Customer>(x => x.Id == customer.Id)), Times.Once);
        Assert.Equivalent(customerModel, customerResponse);
    }
}

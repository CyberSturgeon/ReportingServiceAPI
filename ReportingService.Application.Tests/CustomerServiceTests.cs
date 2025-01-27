
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
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _transactionRepositoryMock = new Mock<ITransactionRepository>();

        var config = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile(new CustomerMapperProfile());
                cfg.AddProfile(new AccountMapperProfile());
                cfg.AddProfile(new TransactionMapperProfile());
                cfg.AddProfile(new ComissionMapperProfile());
            });
        _mapper = new(config);

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
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(id).Result).Returns(customer);
        var customerModel = _mapper.Map<CustomerModel>(customer);

        var customerResponse = _sut.GetCustomerByIdAsync(id).Result;

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
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(id).Result).Returns(customer);
        var customerModel = _mapper.Map<CustomerModel>(customer);

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetFullCustomerByIdAsync(id));

        _customerRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        _accountRepositoryMock.Verify(x => x.FindAsync(x => x.CustomerId == id), Times.Once);
        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetFullCustomerByIdAsync_ExistingUserHaveAccountsNoTransactions_GetSucess()
    {
        var id = Guid.NewGuid();
        var msg = $"Customer {id} have no accounts";
        var customer = new Customer { Id = id };
        var accounts = new List<Account> { new Account { Id = id } };
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(id).Result).Returns(customer);
        _accountRepositoryMock.Setup(x => x.FindAsync(x => x.CustomerId == id).Result).Returns(accounts);   
        var customerModel = _mapper.Map<CustomerModel>(customer);
        var accountModels = _mapper.Map<List<AccountModel>>(accounts);
        customerModel.Accounts = accountModels;

        var customerResponse = _sut.GetFullCustomerByIdAsync(id).Result;

        _customerRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        _accountRepositoryMock.Verify(x => x.FindAsync(x => x.CustomerId == id), Times.Once);
        _transactionRepositoryMock.Verify(x => x.FindAsync(x => x.CustomerId == id), Times.Once);
        Assert.Equivalent(customerModel, customerResponse);
    }

    [Fact]
    public async Task GetFullCustomerByIdAsync_ExistingUserHaveAccountsHaveTransactions_GetSucess()
    {
        var id = Guid.NewGuid();
        var customer = new Customer { Id = id };
        var accounts = new List<Account> { new Account { Id = id } };
        var transactions = new List<Transaction> { new Transaction { Id = id } };
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(id).Result).Returns(customer);
        _accountRepositoryMock.Setup(x => x.FindAsync(x => x.CustomerId == id).Result).Returns(accounts);
        _transactionRepositoryMock.Setup(x => x.FindAsync(x => x.CustomerId == id).Result).Returns(transactions);
        var customerModel = _mapper.Map<CustomerModel>(customer);
        var accountModels = _mapper.Map<List<AccountModel>>(accounts);
        var transactionModels = _mapper.Map<List<TransactionModel>>(transactions);  
        customerModel.Accounts = accountModels;
        customerModel.Transactions = transactionModels;

        var customerResponse = _sut.GetFullCustomerByIdAsync(id).Result;

        _customerRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        _accountRepositoryMock.Verify(x => x.FindAsync(x => x.CustomerId == id), Times.Once);
        _transactionRepositoryMock.Verify(x => x.FindAsync(x => x.CustomerId == id), Times.Once);
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
        _accountRepositoryMock.Setup(x => x.GetByIdAsync(id).Result).Returns(account);

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
        _accountRepositoryMock.Setup(x => x.GetByIdAsync(id).Result).Returns(account);
        _customerRepositoryMock.Setup(x => x.FindAsync(x => x.Accounts.Contains(account)).Result)
            .Returns(new List<Customer> { customer });
        var customerModel = _mapper.Map<CustomerModel>(customer);
        
        var customerResponse = _sut.GetCustomerByAccountIdAsync(id).Result;

        _accountRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        _customerRepositoryMock.Verify(x => x.FindAsync(x => x.Accounts.Contains(account)), Times.Once);
        Assert.Equivalent(customerModel, customerResponse);
    }
}

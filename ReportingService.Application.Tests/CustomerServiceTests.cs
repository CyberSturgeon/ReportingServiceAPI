using AutoMapper;
using Moq;
using ReportingService.Application.Exceptions;
using ReportingService.Application.Mappings;
using ReportingService.Application.Models;
using ReportingService.Application.Services;
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories.Interfaces;
using ReportingService.Application.Tests.TestCases;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ReportingService.Core.Configuration.Filters;

namespace ReportingService.Application.Tests;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mapper _mapper;
    private readonly CustomerService _sut;
    private readonly Mock<ILogger<CustomerService>> _customerServiceLoggerMock;

    public CustomerServiceTests()
    {
        _customerRepositoryMock = new();
        _accountRepositoryMock = new();
        _transactionRepositoryMock = new();
        _customerServiceLoggerMock = new();

        _mapper = new(MapperHelper.ConfigureMapper());

        _sut = new(_customerRepositoryMock.Object, _transactionRepositoryMock.Object,
            _accountRepositoryMock.Object, _mapper, _customerServiceLoggerMock.Object);
    }

    [Theory]
    [InlineData(1, 6, 1, 11, 0)]
    [InlineData(1, 1, 1, 11, 2)]
    [InlineData(1, 27, 2, 28, 3)]
    [InlineData(4, 20, 4, 26, 0)]
    [InlineData(7, 20, 7, 29, 2)]
    [InlineData(1, 1, 12, 29, 17)]
    public async Task GetByBirthAsync_ReturnsCustomersInRange(int startMonth,
        int startDay, int endMonth, int endDay, int expectedCount)
    {
        var customers = new List<Customer>
        {
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1990, 1, 29) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1985, 1, 28) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1990, 1, 2) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1985, 1, 1) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1990, 1, 12) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1985, 1, 15) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1995, 2, 28) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(2026, 8, 1) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1839, 7, 4) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1488, 7, 29) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1234, 7, 28) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1990, 3, 1) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1985, 3, 29) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1995, 3, 2) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1990, 4, 10) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1985, 4, 15) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1995, 4, 3) },
        };

        var mockSet = FakeDbSet.GetMockCustomerDbSet(customers);
        var mockContext = new Mock<DbContext>();
        mockContext.Setup(c => c.Set<Customer>()).Returns(mockSet.Object);

        _customerRepositoryMock.Setup(r => r.FindManyAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
                .ReturnsAsync((Expression<Func<Customer, bool>> predicate) => customers.Where(predicate.Compile()).ToList());

        var dateStart = new DateOnly(DateTime.Now.Year, startMonth, startDay);
        var dateEnd = new DateOnly(DateTime.Now.Year, endMonth, endDay);
        var dates = new DateFilter { DateStart = dateStart, DateEnd = dateEnd };

        var result = await _sut.GetByBirthAsync(dates);

        Assert.Equal(expectedCount, result.Count);
    }

    [Fact]
    public async Task GetFullByIdAsync_NonExistsCustomer_EntityNotFoundExceptionThrown()
    {
        var id = Guid.NewGuid();
        var msg = $"Customer {id} not found";

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetFullByIdAsync(id));

        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetFullByIdAsync_ExistsCustomerNoAccounts_EntityNotFoundExceptionThrown()
    {
        var customer = CustomerTestCase.GetCustomerEntity();
        var id = customer.Id;
        var msg = $"No Accounts related to Customer {id}";
        var customerModel = _mapper.Map<Customer>(customer);
        _customerRepositoryMock.Setup(x => x.FindAsync(x => x.Id == id,
                     It.IsAny<Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>>?>()))
                            .ReturnsAsync(customer);

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetFullByIdAsync(id));

        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetFullByIdAsync_ExistsCustomer_GetSucess()
    {
        var accounts = new List<Account> { AccountTestCase.GetAccountEntity()};
        var customer = CustomerTestCase.GetCustomerEntity(accounts);
        var id = customer.Id;
        accounts[0].CustomerId = id;
        var customerModel = _mapper.Map<Customer>(customer);
        _customerRepositoryMock.Setup(x => x.FindAsync(x => x.Id == id,
                     It.IsAny<Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>>?>()))
                            .ReturnsAsync(customer);
        
        var customerResponse = await _sut.GetFullByIdAsync(id);

        Assert.Equivalent(customerModel, customerResponse);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingCustomer_EntityNotFoundExceptionThrown()
    {
        var id = Guid.NewGuid();
        var msg = $"Customer {id} not found";

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetByIdAsync(id));

        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingCustomer_GetSucess()
    {
        var customer = CustomerTestCase.GetCustomerEntity();
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(customer.Id)).ReturnsAsync(customer);
        var customerModel = _mapper.Map<CustomerModel>(customer);

        var customerResponse = await _sut.GetByIdAsync(customer.Id);

        _customerRepositoryMock.Verify(x => x.GetByIdAsync(customer.Id), Times.Once);
        Assert.Equivalent(customerModel, customerResponse);
    }

    [Fact]
    public async Task GetByAccountIdAsync_NonExistsAccount_EntityNotFoundExceptionThrown()
    {
        var id = Guid.NewGuid();
        var msg = $"Account {id} not found";

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetByAccountIdAsync(id));

        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetByAccountIdAsync_ExistsAccountNonExistsCustomer_EntityNotFoundExceptionThrown()
    {
        var account = AccountTestCase.GetAccountEntity();
        var msg = $"Customer with account {account.Id} not found";
        _accountRepositoryMock.Setup(x => x.GetByIdAsync(account.Id)).ReturnsAsync(account);

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetByAccountIdAsync(account.Id));

        _accountRepositoryMock.Verify(x=> x.GetByIdAsync(account.Id), Times.Once);
        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetByAccountIdAsync_ExistsAccountExistsCustomer_GetSucess()
    {
        var customer = CustomerTestCase.GetCustomerEntity();    
        var account = AccountTestCase.GetAccountEntity(customer.Id, null, customer);
        customer.Accounts.Add(account);
        _accountRepositoryMock.Setup(x => x.GetByIdAsync(account.Id)).ReturnsAsync(account);
        _customerRepositoryMock.Setup(x => x.FindAsync(x => x.Accounts.Contains(account), null))
            .ReturnsAsync(customer);
        var customerModel = _mapper.Map<CustomerModel>(customer);
        
        var customerResponse = await _sut.GetByAccountIdAsync(account.Id);

        _accountRepositoryMock.Verify(x => x.GetByIdAsync(account.Id), Times.Once);
    }

    [Fact]
    public async Task GetByTransactionIdAsync_NonExistsTransaction_EntityNotFoundExceptionThrown()
    {
        var id = Guid.NewGuid();
        var msg = $"Transaction {id} not found";

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetByTransactionIdAsync(id));

        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetByTransactionIdAsync_ExistsTransactionNonExistsCustomer_EntityNotFoundExceptionThrown()
    {
        var transaction = TransactionTestCase.GetTransactionEntity();
        var msg = $"Customer with transaction {transaction.Id} not found";
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(transaction.Id)).ReturnsAsync(transaction);

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetByTransactionIdAsync(transaction.Id));

        _transactionRepositoryMock.Verify(x => x.GetByIdAsync(transaction.Id), Times.Once);
        Assert.Equal(msg , ex.Message);
    }

    [Fact]
    public async Task GetByTransactionIdAsync_ExistsTransactionExistsCustomer_GetSucess()
    {
        var customer = CustomerTestCase.GetCustomerEntity();
        var transaction = TransactionTestCase.GetTransactionEntity(null, customer.Id);
        customer.Transactions.Add(transaction);
        _customerRepositoryMock.Setup(x => x.FindAsync(x => x.Transactions.Contains(transaction),
            null))
            .ReturnsAsync(customer);
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(transaction.Id)).ReturnsAsync(transaction);
        var customerModel = _mapper.Map<CustomerModel>(customer);

        var customerResponse = await _sut.GetByTransactionIdAsync(transaction.Id);

        _transactionRepositoryMock.Verify(x => x.GetByIdAsync(transaction.Id), Times.Once);
        _customerRepositoryMock.Verify(x => x.FindAsync(x => x.Transactions.Contains(transaction),
            null), Times.Once);
        Assert.Equivalent(customerModel, customerResponse);
    }
}

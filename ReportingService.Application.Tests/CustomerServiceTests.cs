﻿using AutoMapper;
using Moq;
using ReportingService.Application.Exceptions;
using ReportingService.Application.Mappings;
using ReportingService.Application.Models;
using ReportingService.Application.Services;
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories.Interfaces;
using ReportingService.Application.Tests.TestCases;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
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

    [Theory]
    [InlineData(3, 4, 4, 4, 3)]
    [InlineData(7, 14, 8, 21, 3)]
    [InlineData(1, 3, 12, 1, 13)]
    [InlineData(6, 4, 8, 20, 3)]
    public async Task GetCustomersByBirthAsync_ReturnsCustomersInRange(int startMonth,
        int startDay, int endMonth, int endDay, int expectedCount)
    {
        var customers = new List<Customer>
        {
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1990, 1, 10) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1985, 1, 15) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1995, 2, 3) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(2026, 8, 21) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1839, 7, 4) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1488, 7, 20) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1234, 7, 14) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1990, 3, 10) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1985, 3, 15) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1995, 3, 3) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1990, 4, 10) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1985, 4, 15) },
            new Customer { Id = Guid.NewGuid(), BirthDate = new DateTime(1995, 4, 3) },
        };

        var mockSet = FakeDbSet.GetMockCustomerDbSet(customers);
        var mockContext = new Mock<DbContext>();
        mockContext.Setup(c => c.Set<Customer>()).Returns(mockSet.Object);

        _customerRepositoryMock.Setup(r => r.FindManyAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
                .ReturnsAsync((Expression<Func<Customer, bool>> predicate) => customers.Where(predicate.Compile()).ToList());

        var dateStart = new DateTime(DateTime.Now.Year, startMonth, startDay);
        var dateEnd = new DateTime(DateTime.Now.Year, endMonth, endDay);

        var result = await _sut.GetCustomersByBirthAsync(dateStart, dateEnd);

        Assert.Equal(expectedCount, result.Count);
    }

    [Fact]
    public async Task GetFullCustomerByIdAsync_NonExistsCustomer_EntityNotFoundExceptionThrown()
    {
        var id = Guid.NewGuid();
        var msg = $"Customer {id} not found";

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetFullCustomerByIdAsync(id));

        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetFullCustomerByIdAsync_ExistsCustomerNoAccounts_EntityNotFoundExceptionThrown()
    {
        var customer = CustomerTestCase.GetCustomerEntity();
        var id = customer.Id;
        var msg = $"No Accounts related to Customer {id}";
        var customerModel = _mapper.Map<Customer>(customer);
        _customerRepositoryMock.Setup(x => x.FindAsync(x => x.Id == id,
                     It.IsAny<Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>>?>()))
                            .ReturnsAsync(customer);

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetFullCustomerByIdAsync(id));

        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetFullCustomerByIdAsync_ExistsCustomer_GetSucess()
    {
        var accounts = new List<Account> { AccountTestCase.GetAccountEntity()};
        var customer = CustomerTestCase.GetCustomerEntity(accounts);
        var id = customer.Id;
        accounts[0].CustomerId = id;
        var customerModel = _mapper.Map<Customer>(customer);
        _customerRepositoryMock.Setup(x => x.FindAsync(x => x.Id == id,
                     It.IsAny<Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>>?>()))
                            .ReturnsAsync(customer);
        
        var customerResponse = await _sut.GetFullCustomerByIdAsync(id);

        Assert.Equivalent(customerModel, customerResponse);
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
        var customer = CustomerTestCase.GetCustomerEntity();
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(customer.Id)).ReturnsAsync(customer);
        var customerModel = _mapper.Map<CustomerModel>(customer);

        var customerResponse = await _sut.GetCustomerByIdAsync(customer.Id);

        _customerRepositoryMock.Verify(x => x.GetByIdAsync(customer.Id), Times.Once);
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
        var account = AccountTestCase.GetAccountEntity();
        var msg = $"Customer with account {account.Id} not found";
        _accountRepositoryMock.Setup(x => x.GetByIdAsync(account.Id)).ReturnsAsync(account);

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetCustomerByAccountIdAsync(account.Id));

        _accountRepositoryMock.Verify(x=> x.GetByIdAsync(account.Id), Times.Once);
        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public async Task GetCustomerByAccountIdAsync_ExistsAccountExistsCustomer_GetSucess()
    {
        var customer = CustomerTestCase.GetCustomerEntity();    
        var account = AccountTestCase.GetAccountEntity(customer.Id, null, customer);
        customer.Accounts.Add(account);
        _accountRepositoryMock.Setup(x => x.GetByIdAsync(account.Id)).ReturnsAsync(account);
        _customerRepositoryMock.Setup(x => x.FindAsync(x => x.Accounts.Contains(account), null))
            .ReturnsAsync(customer);
        var customerModel = _mapper.Map<CustomerModel>(customer);
        
        var customerResponse = await _sut.GetCustomerByAccountIdAsync(account.Id);

        _accountRepositoryMock.Verify(x => x.GetByIdAsync(account.Id), Times.Once);
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
        var transaction = TransactionTestCase.GetTransactionEntity();
        var msg = $"Customer with transaction {transaction.Id} not found";
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(transaction.Id)).ReturnsAsync(transaction);

        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetCustomerByTransactionIdAsync(transaction.Id));

        _transactionRepositoryMock.Verify(x => x.GetByIdAsync(transaction.Id), Times.Once);
        Assert.Equal(msg , ex.Message);
    }

    [Fact]
    public async Task GetCustomerByTransactionIdAsync_ExistsTransactionExistsCustomer_GetSucess()
    {
        var customer = CustomerTestCase.GetCustomerEntity();
        var transaction = TransactionTestCase.GetTransactionEntity(null, customer.Id);
        customer.Transactions.Add(transaction);
        _customerRepositoryMock.Setup(x => x.FindAsync(x => x.Transactions.Contains(transaction),
            null))
            .ReturnsAsync(customer);
        _transactionRepositoryMock.Setup(x => x.GetByIdAsync(transaction.Id)).ReturnsAsync(transaction);
        var customerModel = _mapper.Map<CustomerModel>(customer);

        var customerResponse = await _sut.GetCustomerByTransactionIdAsync(transaction.Id);

        _transactionRepositoryMock.Verify(x => x.GetByIdAsync(transaction.Id), Times.Once);
        _customerRepositoryMock.Verify(x => x.FindAsync(x => x.Transactions.Contains(transaction),
            null), Times.Once);
        Assert.Equivalent(customerModel, customerResponse);
    }

    [Fact]
    public async Task AddCustomerAsync_AddSucess()
    {
        var customer = CustomerTestCase.GetCustomerEntity();
        _customerRepositoryMock.Setup(x =>
            x.AddAndReturnAsync(It.Is<Customer>(x => x.Id == customer.Id))).ReturnsAsync(customer);
        var customerModel = _mapper.Map<CustomerModel>(customer);

        var customerResponse = await _sut.AddCustomerAsync(customerModel);

        _customerRepositoryMock.Verify(x => x.AddAndReturnAsync(It.Is<Customer>(x => x.Id == customer.Id)), Times.Once);
        Assert.Equivalent(customerModel, customerResponse);
    }
}

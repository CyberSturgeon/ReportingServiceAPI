
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
    private readonly Mapper _mapper;
    private readonly CustomerService _sut;

    public CustomerServiceTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();

        var config = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile(new CustomerMapperProfile());
                cfg.AddProfile(new AccountMapperProfile());
                cfg.AddProfile(new TransactionMapperProfile());
                cfg.AddProfile(new ComissionMapperProfile());
            });
        _mapper = new(config);

        _sut = new(_customerRepositoryMock.Object, _mapper);
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

        var customerResponse = _sut.GetCustomerByIdAsync(id);
        _customerRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        Assert.Equivalent(customerModel, customerResponse);
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReportingService.Application.Exceptions;
using ReportingService.Application.Models;
using ReportingService.Application.Services.Interfaces;
using ReportingService.Core.Configuration.Filters;
using ReportingService.Presentanion.Controllers;
using ReportingService.Presentanion.Mappings;
using ReportingService.Presentanion.Models;
using System.Net;

namespace ReportingService.Presentation.Tests;

public class CustomerControllerTests
{
    private readonly Mock<ICustomerService> _customerServiceMock;
    private readonly Mapper _mapper;
    private readonly CustomerController _sut;

    public CustomerControllerTests()
    {
        _customerServiceMock = new();
        var config = new MapperConfiguration(
        cfg =>
        {
            cfg.AddProfile(new CustomerMapperProfile());
        });
        _mapper = new Mapper(config);
        _sut = new(_customerServiceMock.Object, _mapper);
    }

    [Fact]
    public async Task GetByIdAsync_ExistsCustomer_GetSuccess()
    {
        var expectedStatusCode = HttpStatusCode.OK;
        var id = Guid.NewGuid();
        var customerModel = new CustomerModel();
        _customerServiceMock.Setup(t => t.GetByIdAsync(id)).ReturnsAsync(customerModel);

        var result = await _sut.GetByIdAsync(id);
        var statusCode = (result.Result as ObjectResult).StatusCode;

        Assert.IsType<ActionResult<CustomerResponse>>(result);
        Assert.Equal((int)expectedStatusCode, statusCode);
        _customerServiceMock.Verify(t =>
           t.GetByIdAsync(id),
           Times.Once);
    }

    [Fact]
    public async Task GetByBirth_GetSuccess()
    {
        var expectedStatusCode = HttpStatusCode.OK;
        var dates = new DateFilter { DateStart = DateTime.Now, DateEnd = DateTime.Now };
        var customerModels = new List<CustomerModel> { new CustomerModel()};
        _customerServiceMock.Setup(t => t.GetByBirthAsync(dates)).ReturnsAsync(customerModels);

        var result = await _sut.GetByBirthAsync(dates);
        var statusCode = (result.Result as ObjectResult).StatusCode;

        Assert.IsType<ActionResult<List<CustomerResponse>>>(result);
        Assert.Equal((int)expectedStatusCode, statusCode);
        _customerServiceMock.Verify(t =>
           t.GetByBirthAsync(dates),
           Times.Once);
    }
}

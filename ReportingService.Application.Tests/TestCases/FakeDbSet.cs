using Moq;
using Microsoft.EntityFrameworkCore;
using ReportingService.Persistence.Entities;

public static class FakeDbSet
{
    public static Mock<DbSet<Customer>> GetMockCustomerDbSet(List<Customer> customers)
    {
        var mockSet = new Mock<DbSet<Customer>>();
        var queryable = customers.AsQueryable();

        mockSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

        return mockSet;
    }
}
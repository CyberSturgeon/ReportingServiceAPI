using ReportingService.Persistence.Entities;

namespace ReportingService.Persistence.Repositories.Interfaces;

public interface ICustomerRepository : IBaseRepository<Customer>    
{
    Task BatchUpdateRoleAsync(List<Guid> customerIds);
    Task AddCustomerRawSqlAsync(Customer customer);
    Task UpdateCustomerRawSqlAsync(Customer customer);
}
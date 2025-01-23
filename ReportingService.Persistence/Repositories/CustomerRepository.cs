
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories.Interfaces;

namespace ReportingService.Persistence.Repositories;

public class CustomerRepository(ReportingContext context) 
        : BaseRepository<Customer>(context), ICustomerRepository
{
}

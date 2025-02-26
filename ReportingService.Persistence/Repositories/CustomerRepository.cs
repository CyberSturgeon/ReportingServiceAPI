using Microsoft.EntityFrameworkCore;
using MYPBackendMicroserviceIntegrations.Enums;
using Npgsql;
using NpgsqlTypes;
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories.Interfaces;

namespace ReportingService.Persistence.Repositories;

public class CustomerRepository(ReportingContext context) 
        : BaseRepository<Customer>(context), ICustomerRepository
{
    public async Task BatchUpdateRoleAsync(List<Guid> customerIds)
    {
        var vipAccounts = new List<int>()
        { (int)Currency.JPY, (int)Currency.CNY, (int)Currency.RSD, (int)Currency.BGN, (int)Currency.ARS };

        await context.Database.ExecuteSqlRawAsync(
        "SELECT \"BatchUpdateRole\"(@customer_ids, @currencies)",
        new NpgsqlParameter("customer_ids", customerIds.ToArray())
        {
            NpgsqlDbType = NpgsqlDbType.Array | NpgsqlDbType.Uuid
        },
        new NpgsqlParameter("currencies", vipAccounts.ToArray())
        {
            NpgsqlDbType = NpgsqlDbType.Array | NpgsqlDbType.Integer
        });
    }

}

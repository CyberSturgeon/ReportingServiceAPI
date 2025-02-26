using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories.Interfaces;

namespace ReportingService.Persistence.Repositories;

public class AccountRepository(ReportingContext context)
    : BaseRepository<Account>(context), IAccountRepository
{
    public async Task AddAccountRawSqlAsync(Account account)
    {
        await context.Database.ExecuteSqlRawAsync("SELECT InsertAccount(@p_id,@p_customer_id,@p_date_created,@p_currency,@p_is_deactivated)",
             new NpgsqlParameter("p_id", account.Id)
             {
                 NpgsqlDbType = NpgsqlDbType.Uuid
             },
            new NpgsqlParameter("p_customer_id", account.CustomerId)
            {
                NpgsqlDbType = NpgsqlDbType.Uuid
            },
            new NpgsqlParameter("p_date_created", account.DateCreated)
            {
                NpgsqlDbType = NpgsqlDbType.Timestamp
            },
            new NpgsqlParameter("p_currency", (int)account.Currency)
            {
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new NpgsqlParameter("p_is_deactivated", account.IsDeactivated)
            {
                NpgsqlDbType = NpgsqlDbType.Boolean
            }
        );

    }
}
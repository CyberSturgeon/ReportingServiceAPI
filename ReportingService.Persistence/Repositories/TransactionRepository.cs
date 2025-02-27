using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using ReportingService.Persistence.Entities;
using ReportingService.Persistence.Repositories.Interfaces;

namespace ReportingService.Persistence.Repositories;

public class TransactionRepository(ReportingContext context) :
    BaseRepository<Transaction>(context), ITransactionRepository
{
    public async Task AddTransactionRawSqlAsync(Transaction transaction)
    {
        await context.Database.ExecuteSqlRawAsync("SELECT InsertAccount(@p_id,@p_account_id,@p_amount,@p_date," +
            "@p_amount_rub,@p_transaction_type,@p_currency,@p_customer_id )",
             new NpgsqlParameter("p_id", transaction.Id)
             {
                 NpgsqlDbType = NpgsqlDbType.Uuid
             },
            new NpgsqlParameter("p_account_id", transaction.AccountId)
            {
                NpgsqlDbType = NpgsqlDbType.Uuid
            },
            new NpgsqlParameter("p_amount", transaction.Amount)
            {
                NpgsqlDbType = NpgsqlDbType.Numeric
            },
            new NpgsqlParameter("p_date", transaction.Date)
            {
                NpgsqlDbType = NpgsqlDbType.TimestampTz
            },
            new NpgsqlParameter("p_amount_rub", transaction.AmountRUB)
            {
                NpgsqlDbType = NpgsqlDbType.Numeric
            },
            new NpgsqlParameter("p_transaction_type", (int)transaction.TransactionType)
            {
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new NpgsqlParameter("p_currency", (int)transaction.Currency)
            {
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new NpgsqlParameter("p_customer_id", transaction.CustomerId)
            {
                NpgsqlDbType = NpgsqlDbType.Uuid
            }


        );

    }
}

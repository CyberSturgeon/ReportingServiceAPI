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

    public async Task AddCustomerRawSqlAsync(Customer customer)
    {
        await context.Database.ExecuteSqlRawAsync("SELECT InsertCustomer(@p_role,@p_phone,@p_address,@p_password,@p_birth_date,@p_first_name,@p_last_name,@p_is_deactivated,@p_email,@p_custom_vip_due_date,@p_customer_service_id)",
             new NpgsqlParameter("p_role", (int)customer.Role)
             {
                 NpgsqlDbType =NpgsqlDbType.Integer
             },
            new NpgsqlParameter("p_phone", customer.Phone)
            {
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new NpgsqlParameter("p_address", customer.Address)
            {
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new NpgsqlParameter("p_password", customer.Password)
            {
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new NpgsqlParameter("p_birth_date", customer.BirthDate)
            {
                NpgsqlDbType = NpgsqlDbType.TimestampTz
            },
            new NpgsqlParameter("p_first_name", customer.FirstName)
            {
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new NpgsqlParameter("p_last_name", customer.LastName)
            {
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new NpgsqlParameter("p_is_deactivated", customer.IsDeactivated)
            {
                NpgsqlDbType = NpgsqlDbType.Boolean
            },
            new NpgsqlParameter("p_email", customer.Email)
            {
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new NpgsqlParameter("p_custom_vip_due_date", DateTime.Now)
            {
                NpgsqlDbType = NpgsqlDbType.Timestamp
            },
            new NpgsqlParameter("p_customer_service_id", customer.CustomerServiceId)
            {
                NpgsqlDbType = NpgsqlDbType.Uuid
            }
            );
    }

    public async Task UpdateCustomerRawSqlAsync( Customer customer )
    {
        await context.Database.ExecuteSqlRawAsync("SELECT InsertCustomer(@p_customer_service_id,@p_role,@p_phone,@p_address,@p_password,@p_birth_date,@p_first_name,@p_last_name,@p_is_deactivated,@p_email,@p_custom_vip_due_date)",
             new NpgsqlParameter("p_role", (int)customer.Role)
             {
                 NpgsqlDbType = NpgsqlDbType.Integer
             },
            new NpgsqlParameter("p_phone", customer.Phone)
            {
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new NpgsqlParameter("p_address", customer.Address)
            {
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new NpgsqlParameter("p_password", customer.Password)
            {
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new NpgsqlParameter("p_birth_date", customer.BirthDate)
            {
                NpgsqlDbType = NpgsqlDbType.TimestampTz
            },
            new NpgsqlParameter("p_first_name", customer.FirstName)
            {
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new NpgsqlParameter("p_last_name", customer.LastName)
            {
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new NpgsqlParameter("p_is_deactivated", customer.IsDeactivated)
            {
                NpgsqlDbType = NpgsqlDbType.Boolean
            },
            new NpgsqlParameter("p_email", customer.Email)
            {
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new NpgsqlParameter("p_custom_vip_due_date", DateTime.Now)
            {
                NpgsqlDbType = NpgsqlDbType.Timestamp
            },
            new NpgsqlParameter("p_customer_service_id", customer.CustomerServiceId)
            {
                NpgsqlDbType = NpgsqlDbType.Uuid
            }
            );
    }
}

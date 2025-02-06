using ReportingService.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using ReportingService.Persistence.Entities; 

namespace ReportingService.Persistence;

public class ReportingContext (DbContextOptions<ReportingContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Comission> Comissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureAccount();
        modelBuilder.ConfigureCustomer();
        modelBuilder.ConfigureTransaction();
        modelBuilder.ConfigureComission();
    }
}

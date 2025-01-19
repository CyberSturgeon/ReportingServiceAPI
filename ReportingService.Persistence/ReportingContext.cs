
using Microsoft.EntityFrameworkCore;
using ReportingService.Persistence.Entities;

namespace ReportingService.Persistence;

public class ReportingContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Comission> Comissions { get; set; }
    public ReportingContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}

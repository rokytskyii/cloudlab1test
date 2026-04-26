using Microsoft.EntityFrameworkCore;
using SmartphoneStore.Dal.Tablet;

namespace SmartphoneStore.Dal;

public class CosmosDbContext : DbContext
{
    public CosmosDbContext(DbContextOptions<CosmosDbContext> options) : base(options)
    {
    }

    public DbSet<TabletDao> Tablets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TabletDao>()
            .ToContainer("Tablets")
            .HasPartitionKey(c => c.Id);
    }
}
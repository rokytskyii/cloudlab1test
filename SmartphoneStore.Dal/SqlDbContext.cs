using Microsoft.EntityFrameworkCore;
using SmartphoneStore.Dal.Smartphone;

namespace SmartphoneStore.Dal;

public class SqlDbContext : DbContext
{
    public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options)
    {
    }

    public DbSet<SmartphoneDao> Smartphones { get; set; }
}
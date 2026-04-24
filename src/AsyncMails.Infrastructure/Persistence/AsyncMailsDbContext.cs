using Microsoft.EntityFrameworkCore;

namespace AsyncMails.Infrastructure.Persistence;

/// <summary>
/// Main database context for the AsyncMails application.
/// DbSets will be added here as domain entities are created.
/// </summary>
public class AsyncMailsDbContext : DbContext
{
    public AsyncMailsDbContext(DbContextOptions<AsyncMailsDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Future: Apply entity configurations from this assembly
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(AsyncMailsDbContext).Assembly);
    }
}

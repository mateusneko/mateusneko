using FieldOpsAI.Core.Domain;
using Microsoft.EntityFrameworkCore;
namespace FieldOpsAI.Infrastructure.Data;
public sealed class IncidentsDbContext(DbContextOptions<IncidentsDbContext> options) : DbContext(options)
{
    public DbSet<Incident> Incidents => Set<Incident>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Incident>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.CreatedAtUtc);
            e.Property(x => x.Location).HasMaxLength(160).IsRequired();
            e.Property(x => x.Description).HasMaxLength(2000).IsRequired();
            e.Property(x => x.Status).HasConversion<string>().HasMaxLength(15);
            e.Property(x => x.Priority).HasConversion<string>().HasMaxLength(15);
            e.Property(x => x.Category).HasConversion<string>().HasMaxLength(20);
            e.Property(x => x.Summary).HasMaxLength(500);
            e.Property(x => x.Guidance).HasMaxLength(1000);
            e.Property(x => x.TriageSource).HasMaxLength(30);
        });
    }
}

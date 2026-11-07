namespace ServerMonitor.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using ServerMonitor.Domain.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<MonitoredServer> Servers => Set<MonitoredServer>();
    public DbSet<HealthCheck> HealthChecks => Set<HealthCheck>();
    public DbSet<Incident> Incidents => Set<Incident>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MonitoredServer>()
            .HasIndex(s => s.Url)
            .IsUnique();

        modelBuilder.Entity<HealthCheck>()
            .HasOne(h => h.Server)
            .WithMany(s => s.HealthChecks)
            .HasForeignKey(h => h.ServerId);

        modelBuilder.Entity<Incident>()
            .HasOne(i => i.Server)
            .WithMany(s => s.Incidents)
            .HasForeignKey(i => i.ServerId);

        modelBuilder.Entity<HealthCheck>()
            .HasIndex(h => h.CheckedAt);
    }
}

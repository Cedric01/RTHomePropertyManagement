using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace RTHomePropertyManagement.Models;

public class RealEstateDbContext : DbContext
{
    public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options)
        : base(options)
    {
    }

    public DbSet<Property> Properties => Set<Property>();
    public DbSet<Agent> Agents => Set<Agent>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<ListingType> ListingTypes => Set<ListingType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("core");

        modelBuilder.Entity<Property>()
            .HasOne(p => p.ListingType)
            .WithMany(lt => lt.Properties)
            .HasForeignKey(p => p.ListingTypeId);

        modelBuilder.Entity<Property>()
            .HasOne(p => p.Agent)
            .WithMany(a => a.Properties)
            .HasForeignKey(p => p.AgentId);

        modelBuilder.Entity<Property>()
            .HasOne(p => p.Location)
            .WithMany(l => l.Properties)
            .HasForeignKey(p => p.LocationId);

        base.OnModelCreating(modelBuilder);
    }

}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RTHomePropertyManagement.Models;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public virtual DbSet<Property> Properties { get; set; } // Make this virtual
    public virtual DbSet<PropertyListing> PropertiesListing { get; set; }
}

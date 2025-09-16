using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RTHomePropertyManagement.Models;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    private DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Property> Properties { get; set; }
}

using Titan.Domain.Entities.User;
using Titan.Domain.Entities;
using Titan.Infrastructure.Data.Configurations;

namespace Titan.Infrastructure.Data;

public class TitanDbContext(DbContextOptions<TitanDbContext> options) : DbContext(options)
{
    public DbSet<Company> Companies { get; set; }
    
    public DbSet<CompanyUser> CompanyUsers { get; set; }
    
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserBase>().UseTptMappingStrategy();

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CompanyConfiguration).Assembly);
    }
}

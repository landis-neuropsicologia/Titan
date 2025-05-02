using Titan.Domain.Entities;

namespace Titan.Infrastructure.Data.Configurations;

internal class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("tblRoles");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id).ValueGeneratedOnAdd();
        builder.Property(r => r.Name).HasMaxLength(50).IsRequired();
        builder.Property(r => r.Description).HasMaxLength(256);

        // Garanta que nomes de roles sejam únicos
        builder.HasIndex(r => r.Name).IsUnique();

        // Seed de roles iniciais
        var companyUserRole = new Role("company_user", "Regular company user");
        var companyAdminRole = new Role("company_admin", "Company administrator");

        // Use reflection to set the Id properties for seeding
        typeof(Role).GetProperty("Id")?.SetValue(companyUserRole, 1);
        typeof(Role).GetProperty("Id")?.SetValue(companyAdminRole, 2);

        builder.HasData(companyUserRole, companyAdminRole);
    }
}

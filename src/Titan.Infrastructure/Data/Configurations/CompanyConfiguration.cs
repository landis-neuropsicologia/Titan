using Titan.Domain.Entities;
using Titan.Domain.ValueObjects;

namespace Titan.Infrastructure.Data.Configurations;

internal class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("tblCompanies");
        
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedNever();
        builder.Property(c => c.CreatedAt).IsRequired();

        // Configure as Value Object
        builder.Property(c => c.Name).HasConversion(name => name.Value, value => Name.Create(value)).HasMaxLength(256).IsRequired();
        builder.Property(c => c.CommercialName).HasConversion(name => name.Value, value => Name.Create(value)).HasMaxLength(256).IsRequired(false);
        builder.Property(c => c.TaxNumber).HasConversion(name => name.Value, value => TaxNumber.Create(value)).HasMaxLength(16).IsRequired();
        builder.Property(c => c.Domain).HasConversion(domain => domain != null ? domain.Value : null, value => value != null ? DomainName.Create(value) : null).HasMaxLength(256).IsRequired(false);

        // Relationship with CompanyUsers
        builder.HasMany(c => c.Users).WithOne().HasForeignKey(u => u.CompanyId).OnDelete(DeleteBehavior.Cascade);

        // Índice para buscas mais rápidas
        builder.HasIndex(u => u.TaxNumber).IsUnique().HasDatabaseName("IDX_Companies_TaxNumber");
        builder.HasIndex(u => u.Name).HasDatabaseName("IDX_Companies_Name");
    }
}

using Titan.Domain.Entities.User;
using Titan.Domain.ValueObjects;

namespace Titan.Infrastructure.Data.Configurations;

internal class StaffUserConfiguration : IEntityTypeConfiguration<StaffUser>
{
    public void Configure(EntityTypeBuilder<StaffUser> builder)
    {
        // Definição da tabela específica para StaffUser
        builder.ToTable("StaffUsers");

        // Configure como Value Object
        builder.Property(u => u.EmployeeId).HasConversion(id => id != null ? id.Value : null, value => value != null ? EmployeeId.Create(value) : null).HasMaxLength(50).IsRequired();
        builder.Property(u => u.Department).HasMaxLength(100);
    }
}

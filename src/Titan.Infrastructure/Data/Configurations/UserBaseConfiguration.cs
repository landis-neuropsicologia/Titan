using Titan.Domain.Entities.User;
using Titan.Domain.ValueObjects;

namespace Titan.Infrastructure.Data.Configurations;

internal class UserBaseConfiguration : IEntityTypeConfiguration<UserBase>
{
    public void Configure(EntityTypeBuilder<UserBase> builder)
    {
        builder.ToTable("tblUsers");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).ValueGeneratedNever();
        builder.Property(u => u.IsActive).IsRequired();
        builder.Property(u => u.CreatedAt).IsRequired();
        builder.Property(u => u.LastLogin);

        // Configure como Value Object
        builder.Property(u => u.Name).HasConversion(name => name != null ? name.Value : null, value => value != null ? Name.Create(value) : null).HasMaxLength(256).IsRequired();
        builder.Property(u => u.Email).HasConversion(email => email != null ? email.Value : null, value => value != null ? Email.Create(value) : null).HasMaxLength(256).IsRequired();

        // Índice para buscas mais rápidas
        builder.HasIndex(u => u.Email).IsUnique().HasDatabaseName("IDX_Users_Email");
    }
}

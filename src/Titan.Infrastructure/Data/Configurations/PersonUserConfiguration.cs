using Titan.Domain.Entities.User;
using Titan.Domain.ValueObjects;

namespace Titan.Infrastructure.Data.Configurations;

internal class PersonUserConfiguration : IEntityTypeConfiguration<PersonUser>
{
    public void Configure(EntityTypeBuilder<PersonUser> builder)
    {
        builder.ToTable("PersonUsers");

        // Configure ActivationKey como Value Object
        builder.Property(u => u.ActivationKey).HasConversion(key => key != null ? key.Value : null, value => value != null ? ActivationKey.Create(value) : null).HasMaxLength(32);
        builder.Property(u => u.RegisteredViaSocialMedia).IsRequired();

        // Configure SocialMediaProvider como Value Object (se for um Value Object)
        builder.Property(u => u.SocialMediaProvider).HasMaxLength(50);
    }
}

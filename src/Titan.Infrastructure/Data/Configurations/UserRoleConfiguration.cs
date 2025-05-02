using Titan.Domain.Entities;
using Titan.Domain.Entities.User;

namespace Titan.Infrastructure.Data.Configurations;

internal class UserRoleConfiguration : IEntityTypeConfiguration<UserBase>
{
    public void Configure(EntityTypeBuilder<UserBase> builder)
    {
        builder.ToTable("tblUsers");

        // Configure o relacionamento muitos-para-muitos entre UserBase e Role
        builder.HasMany(u => u.Roles).WithMany(r => r.Users).UsingEntity<Dictionary<string, object>>("UserRoles",
                r => r.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                l => l.HasOne<UserBase>().WithMany().HasForeignKey("UserId"),
                static j =>
                {
                    j.ToTable("UserRoles");
                    j.HasKey("UserId", "RoleId");
                    j.HasIndex("RoleId");
                });
    }
}

using Application.Models.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GorjetaFacil.Repository.Mapping
{
    public class UserRoleMapping : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_role");

            builder.HasKey(k => new
            {
                k.RoleId,
                k.UserId
            });

            builder.Property(p => p.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(p => p.RoleId)
                .HasColumnName("role_id")
                .IsRequired();
        }
    }
}

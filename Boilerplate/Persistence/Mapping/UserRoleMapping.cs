using Application.Models.Entities.Authentication;
using Application.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GorjetaFacil.Repository.Mapping
{
    public class UserRoleMapping : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_role");

            builder.HasNoKey();

            builder.Property(p => p.UserId)
                .HasColumnName("user_id")
                //.HasUintConversion()
                .IsRequired();

            builder.Property(p => p.RoleId)
                .HasColumnName("role_id")
                .HasColumnType("varchar(36)")
                .IsRequired();
        }
    }
}

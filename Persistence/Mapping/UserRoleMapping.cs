using Entities.Login;
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
                .HasColumnName("varchar(36)")
                .IsRequired();

            builder.Property(p => p.RoleId)
                .HasColumnName("varchar(36)")
                .IsRequired();
        }
    }
}

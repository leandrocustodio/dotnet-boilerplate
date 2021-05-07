using Application.Models.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GorjetaFacil.Repository.Mapping
{
    public class RoleMapping : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("role");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
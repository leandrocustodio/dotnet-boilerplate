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
                //.HasColumnType("varchar(36)")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(p => p.Name)
                //.HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
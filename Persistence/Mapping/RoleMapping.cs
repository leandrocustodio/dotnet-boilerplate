using Entities.Login;
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
                .HasColumnName("varchar(36)")
                .IsRequired();

            builder.Property(p => p.Name)
                .HasColumnName("varchar(255)")
                .IsRequired();
        }
    }
}
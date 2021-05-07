using Application.Models.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Mapping
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            builder.HasKey(p => p.Id);

            builder.Ignore(p => p.Roles);

            builder.Property(p => p.Id)
                .IsRequired();

            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.LastName)
                .HasColumnName("last_name");

            builder.Property(p => p.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.Password)
                .HasColumnType("tinytext")
                .IsRequired();

            builder.Property(p => p.RestorePasswordCode)
                .HasColumnName("restore_password_code")
                .HasMaxLength(10);

            builder.Property(p => p.IncorrectAttempts)
                .HasColumnName("incorrect_attempts");

            builder.Property(p => p.IsActive)
                .HasColumnName("is_active")
                .HasColumnType("boolean");

            builder.Property(p => p.IsBlocked)
                .HasColumnName("is_blocked");

            builder.Property(p => p.Created)
                .HasColumnType("datetime");

            builder.Property(p => p.Updated)
                .HasColumnType("datetime");
        }
    }
}

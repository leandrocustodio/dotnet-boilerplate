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
               
                //.HasUintConversion()
                .IsRequired();

            builder.Property(p => p.Name)
                //.HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.LastName)
                .HasColumnName("last_name");
                //.HasColumnType("tinytext");

            builder.Property(p => p.Email)
                //.HasColumnType("char(255)")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.Password)
                .HasColumnType("tinytext")
                .IsRequired();

            builder.Property(p => p.RestorePasswordCode)
                .HasColumnName("restore_password_code")
                //.HasColumnType("varchar(10)")
                .HasMaxLength(10);

            builder.Property(p => p.IncorrectAttempts)
                .HasColumnName("incorrect_attempts");
                //.HasUshortConversion();

            builder.Property(p => p.IsActive)
                .HasColumnName("is_active")
                .HasColumnType("boolean");

            builder.Property(p => p.IsBlocked)
                .HasColumnName("is_blocked");
                //.HasColumnType("boolean");

            builder.Property(p => p.Created)
                .HasColumnType("datetime");

            builder.Property(p => p.Updated)
                .HasColumnType("datetime");
        }
    }
}

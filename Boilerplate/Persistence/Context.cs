using Application.Models.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Application.Persistence
{
    public class Context : DbContext
    {
        public Context() : base() { }
        public Context(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

    }
}

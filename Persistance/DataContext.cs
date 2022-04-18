using Microsoft.EntityFrameworkCore;
using Domain.User;

namespace Persistance
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LastUserView> UsersViews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(user => user.FirstName).HasDefaultValue(string.Empty);
            modelBuilder.Entity<User>().Property(user => user.SecondName).HasDefaultValue(string.Empty);
            modelBuilder.Entity<User>().Property(user => user.PasswordHash).HasConversion(hash => hash, password => BCrypt.Net.BCrypt.HashPassword(password));
        }
    }
}
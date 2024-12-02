using Microsoft.EntityFrameworkCore;
using BackEnd.Models;

namespace BackEnd.Models
{
    public class BackendDbContext : DbContext
    {
        public BackendDbContext(DbContextOptions<BackendDbContext> options)
            : base(options)
        {
        }

        public DbSet<User>? Users { get; set; }
        public DbSet<UserData>? UserDatas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=backend.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserData>()
                .Property(e => e.Date)
                .HasConversion(
                    v => v.ToString("yyyy-MM-dd"),
                    v => DateTime.Parse(v));
        }
    }

    public class User
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }

    public class UserData
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public int Value { get; set; }
    }
}
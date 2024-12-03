using Microsoft.EntityFrameworkCore;
using System;

namespace BackEnd.Models
{
    public class BackendDbContext : DbContext
    {
        public BackendDbContext(DbContextOptions<BackendDbContext> options)
            : base(options)
        {
            Users = Set<User>();
            UserDatas = Set<UserData>();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserData> UserDatas { get; set; }

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
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class UserData
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int Value { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
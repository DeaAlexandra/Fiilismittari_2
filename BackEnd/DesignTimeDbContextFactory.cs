using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.IO;

namespace BackEnd.Models
{

    public class BackEndDbContext : DbContext
    {
        public BackEndDbContext(DbContextOptions<BackEndDbContext> options)
            : base(options)
        {
        }
    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("AppConnection");

            builder.UseSqlite(connectionString);

            return new ApplicationDbContext(builder.Options);
        }
    }

    public class BackEndDbContextFactory : IDesignTimeDbContextFactory<BackEndDbContext>
    {
        public BackEndDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<BackEndDbContext>();
            var connectionString = configuration.GetConnectionString("BackEndConnection");

            builder.UseSqlite(connectionString);

            return new BackEndDbContext(builder.Options);
        }
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using BackEnd.Models;

namespace AdminTool
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                while (true)
                {
                    Console.WriteLine("Valitse toiminto:");
                    Console.WriteLine("1. Lisää admin-käyttäjä");
                    Console.WriteLine("2. Näytä kaikki käyttäjät");
                    Console.WriteLine("3. Vahvista käyttäjän sähköposti");
                    Console.WriteLine("4. Lopeta");

                    Console.Write("Valitse toiminto: ");
                    var valinta = Console.ReadLine();

                    if (valinta == "1")
                    {
                        await AddAdminUser(userManager, roleManager);
                    }
                    else if (valinta == "2")
                    {
                        await ShowAllUsers(userManager);
                    }
                    else if (valinta == "3")
                    {
                        await ConfirmUserEmail(userManager);
                    }
                    else if (valinta == "4")
                    {
                        break;
                    }
                }
            }
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("AppConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddLogging();
        }

        private static async Task AddAdminUser(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminEmail = "admin@example.com";
            var adminPassword = "Admin@123";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    if (!await roleManager.RoleExistsAsync("Admin"))
                    {
                        await roleManager.CreateAsync(new IdentityRole("Admin"));
                    }

                    await userManager.AddToRoleAsync(adminUser, "Admin");

                    Console.WriteLine("Admin user created successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to create admin user:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Admin user already exists.");
            }
        }

        private static async Task ShowAllUsers(UserManager<IdentityUser> userManager)
        {
            var users = userManager.Users;
            foreach (var user in users)
            {
                Console.WriteLine($"User: {user.UserName}, Email: {user.Email}, EmailConfirmed: {user.EmailConfirmed}");
            }
        }

        private static async Task ConfirmUserEmail(UserManager<IdentityUser> userManager)
        {
            Console.Write("Syötä käyttäjän sähköposti: ");
            var email = Console.ReadLine();

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                Console.WriteLine("Käyttäjää ei löytynyt.");
                return;
            }

            user.EmailConfirmed = true;
            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                Console.WriteLine("Käyttäjän sähköposti vahvistettu.");
            }
            else
            {
                Console.WriteLine("Sähköpostin vahvistaminen epäonnistui:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Description}");
                }
            }
        }
    }
}
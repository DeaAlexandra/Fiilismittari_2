using BackEnd.Models;
using BackEnd.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Tarkista komentoriviparametrit
if (args.Length > 0 && args[0] == "cli")
{
    await RunCommandLineInterface(builder);
    return;
}

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=backend.db";

builder.Services.AddSqlite<BackendDbContext>(connectionString);
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<BackendDbContext>();
builder.Services.AddScoped<MoodDataService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fiilismittari API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fiilismittari API v1"));
}

app.UseHttpsRedirection();

app.Run();

// Refactored CLI interface with dependency injection
static async Task RunCommandLineInterface(WebApplicationBuilder builder)
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var backendDbContext = serviceProvider.GetRequiredService<BackendDbContext>();

    await Task.Run(() =>
    {
        while (true)
        {
            Console.WriteLine("Valitse toiminto:");
            Console.WriteLine("1. Lisää käyttäjä");
            Console.WriteLine("2. Lisää mielialadataa");
            Console.WriteLine("3. Lopeta");

            Console.Write("Valinta: ");
            var valinta = Console.ReadLine();

            switch (valinta)
            {
                case "1":
                    LisaaKayttaja.LisaaUusiKayttaja(userManager);
                    break;
                case "2":
                    MoodDataHandler.LisaaTietoja(backendDbContext);
                    break;
                case "3":
                    Console.WriteLine("Ohjelma lopetetaan.");
                    return;
                default:
                    Console.WriteLine("Virheellinen valinta.");
                    break;
            }
        }
    });
}
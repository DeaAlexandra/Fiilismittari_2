using BackEnd.Models;
using BackEnd.Extensions;
using Microsoft.EntityFrameworkCore;
using BackEnd;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Tarkista komentoriviparametrit
if (args.Length > 0 && args[0] == "cli")
{
    await RunCommandLineInterface(args);
    return;
}

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=backend.db";

builder.Services.AddSqlite<BackendDbContext>(connectionString);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Call the UserEndpoints method
app.MapUserDataEndpoints();
app.MapUserEndpoints();

app.Run();

static async Task RunCommandLineInterface(string[] args)
{
    await Task.Run(() =>
    {
        while (true)
        {
            Console.WriteLine("Valitse toiminto:");
            Console.WriteLine("1. Lisää käyttäjä");
            Console.WriteLine("2. Lopeta");

            Console.Write("Valinta: ");
            var valinta = Console.ReadLine();

            switch (valinta)
            {
                case "1":
                    LisaaKayttaja.LisaaUusiKayttaja();
                    break;
                case "2":
                    Console.WriteLine("Ohjelma lopetetaan.");
                    return;
                default:
                    Console.WriteLine("Virheellinen valinta.");
                    break;
            }
        }
    });
}
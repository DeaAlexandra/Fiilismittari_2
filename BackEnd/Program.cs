using BackEnd.Models;
using BackEnd.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
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

// API-päätepisteet fiilismittarin hallintaan
app.MapGet("/api/mooddata", async (MoodDataService moodDataService, UserManager<IdentityUser> userManager, HttpContext httpContext) =>
{
    var userId = userManager.GetUserId(httpContext.User);
    if (userId == null)
    {
        return Results.BadRequest("User ID not found.");
    }

    var moodData = await moodDataService.GetTodayMoodData(userId);

    if (moodData == null)
    {
        return Results.NotFound("User data not found.");
    }

    return Results.Ok(moodData.Value);
});

app.MapPost("/api/mooddata", async (MoodDataService moodDataService, UserData userData) =>
{
    if (userData == null)
    {
        return Results.BadRequest("Invalid user data.");
    }

    await moodDataService.SaveMoodValue(userData.UserId, userData.Value);
    return Results.Ok(userData);
});

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
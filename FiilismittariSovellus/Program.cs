using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using BackEnd.Extensions;
using BackEnd.Services;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Add services to the container.
var appConnectionString = builder.Configuration.GetConnectionString("AppConnection") ?? throw new InvalidOperationException("Connection string 'AppConnection' not found.");
var backendConnectionString = builder.Configuration.GetConnectionString("BackEndConnection") ?? throw new InvalidOperationException("Connection string 'BackEndConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(appConnectionString));
builder.Services.AddDbContext<BackendDbContext>(options =>
    options.UseSqlite(backendConnectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddHttpClient("MoodMeterClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001"); // API:n osoite
});

builder.Services.AddScoped<MoodMeterService>();
builder.Services.AddScoped<MoodDataService>(); // Lisää tämä

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapUserDataEndpoints();

app.Run();
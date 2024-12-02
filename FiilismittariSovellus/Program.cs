using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using BackEnd.Extensions;

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
builder.Services.AddDbContext<BackEndDbContext>(options =>
    options.UseSqlite(backendConnectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

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

// Call the ApplicationDbEndpoints method
app.MapApplicationDbEndpoints();

app.Run();
using BackEnd.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace BackEnd.Extensions
{
    public static class ApplicationDbEndpoints
    {
        public static void MapApplicationDbEndpoints(this IEndpointRouteBuilder endpoints)
        {
            // User endpoints
            endpoints.MapGet("/aspnetusers", async (HttpContext context) =>
            {
                var db = context.RequestServices.GetRequiredService<ApplicationDbContext>();
                return db.Users != null ? Results.Ok(await db.Users.ToListAsync()) : Results.NotFound();
            })
            .WithName("GetAspNetUsers")
            .WithOpenApi();

            endpoints.MapGet("/aspnetusers/{id}", async (string id, ApplicationDbContext db) =>
            {
                if (db.Users == null)
                {
                    return Results.NotFound();
                }
                var user = await db.Users.FindAsync(id);
                return user != null ? Results.Ok(user) : Results.NotFound();
            })
            .WithName("GetAspNetUserById")
            .WithOpenApi();

            endpoints.MapPost("/aspnetusers", async (IdentityUser user, ApplicationDbContext db) =>
            {
                if (db.Users == null)
                {
                    return Results.NotFound();
                }
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return Results.Created($"/aspnetusers/{user.Id}", user);
            })
            .WithName("CreateAspNetUser")
            .WithOpenApi();

            endpoints.MapPut("/aspnetusers/{id}", async (string id, IdentityUser updatedUser, ApplicationDbContext db) =>
            {
                if (db.Users == null)
                {
                    return Results.NotFound();
                }
                var user = await db.Users.FindAsync(id);
                if (user == null)
                {
                    return Results.NotFound();
                }

                user.UserName = updatedUser.UserName;
                user.NormalizedUserName = updatedUser.NormalizedUserName;
                user.Email = updatedUser.Email;
                user.NormalizedEmail = updatedUser.NormalizedEmail;
                user.EmailConfirmed = updatedUser.EmailConfirmed;
                user.PasswordHash = updatedUser.PasswordHash;
                user.SecurityStamp = updatedUser.SecurityStamp;
                user.ConcurrencyStamp = updatedUser.ConcurrencyStamp;
                user.PhoneNumber = updatedUser.PhoneNumber;
                user.PhoneNumberConfirmed = updatedUser.PhoneNumberConfirmed;
                user.TwoFactorEnabled = updatedUser.TwoFactorEnabled;
                user.LockoutEnd = updatedUser.LockoutEnd;
                user.LockoutEnabled = updatedUser.LockoutEnabled;
                user.AccessFailedCount = updatedUser.AccessFailedCount;

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateAspNetUser")
            .WithOpenApi();

            endpoints.MapDelete("/aspnetusers/{id}", async (string id, ApplicationDbContext db) =>
            {
                if (db.Users == null)
                {
                    return Results.NotFound();
                }
                var user = await db.Users.FindAsync(id);
                if (user == null)
                {
                    return Results.NotFound();
                }

                db.Users.Remove(user);
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("DeleteAspNetUser")
            .WithOpenApi();
        }
    }
}
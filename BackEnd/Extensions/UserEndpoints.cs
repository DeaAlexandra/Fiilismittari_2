using BackEnd.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BackEnd.Extensions
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder endpoints)
        {
            // User endpoints
            endpoints.MapGet("/users", async (HttpContext context) =>
            {
                var db = context.RequestServices.GetRequiredService<BackendDbContext>();
                return db.Users != null ? Results.Ok(await db.Users.ToListAsync()) : Results.NotFound();
            })
            .WithName("GetUsers")
            .WithOpenApi();

            endpoints.MapGet("/users/{id}", async (int id, BackendDbContext db) =>
            {
                if (db.Users == null)
                {
                    return Results.NotFound();
                }
                var user = await db.Users.FindAsync(id);
                return user != null ? Results.Ok(user) : Results.NotFound();
            })
            .WithName("GetUserById")
            .WithOpenApi();

            endpoints.MapPost("/users", async (User user, BackendDbContext db) =>
            {
                if (db.Users == null)
                {
                    return Results.NotFound();
                }
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return Results.Created($"/users/{user.Id}", user);
            })
            .WithName("CreateUser")
            .WithOpenApi();

            endpoints.MapPut("/users/{id}", async (int id, User updatedUser, BackendDbContext db) =>
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

                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateUser")
            .WithOpenApi();

            endpoints.MapDelete("/users/{id}", async (int id, BackendDbContext db) =>
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
            .WithName("DeleteUser")
            .WithOpenApi();
        }
    }
}
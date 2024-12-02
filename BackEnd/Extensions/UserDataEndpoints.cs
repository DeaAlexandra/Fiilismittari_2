using BackEnd.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BackEnd.Extensions
{
    public static class UserDataEndpoints
    {
        public static void MapUserDataEndpoints(this IEndpointRouteBuilder endpoints)
        {
            // UserData endpoints
            endpoints.MapGet("/userdatas", async (HttpContext context) =>
            {
                var db = context.RequestServices.GetRequiredService<BackendDbContext>();
                return db.UserDatas != null ? Results.Ok(await db.UserDatas.ToListAsync()) : Results.NotFound();
            })
            .WithName("GetUserDatas")
            .WithOpenApi();

            endpoints.MapGet("/userdatas/{id}", async (int id, BackendDbContext db) =>
            {
                if (db.UserDatas == null)
                {
                    return Results.NotFound();
                }
                var userData = await db.UserDatas.FindAsync(id);
                return userData != null ? Results.Ok(userData) : Results.NotFound();
            })
            .WithName("GetUserDataById")
            .WithOpenApi();

            endpoints.MapPost("/userdatas", async (UserData userData, BackendDbContext db) =>
            {
                if (db.UserDatas == null)
                {
                    return Results.NotFound();
                }
                db.UserDatas.Add(userData);
                await db.SaveChangesAsync();
                return Results.Created($"/userdatas/{userData.Id}", userData);
            })
            .WithName("CreateUserData")
            .WithOpenApi();

            endpoints.MapPut("/userdatas/{id}", async (int id, UserData updatedUserData, BackendDbContext db) =>
            {
                if (db.UserDatas == null)
                {
                    return Results.NotFound();
                }
                var userData = await db.UserDatas.FindAsync(id);
                if (userData == null)
                {
                    return Results.NotFound();
                }

                userData.UserId = updatedUserData.UserId;
                userData.Date = updatedUserData.Date;
                userData.Value = updatedUserData.Value;

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateUserData")
            .WithOpenApi();

            endpoints.MapDelete("/userdatas/{id}", async (int id, BackendDbContext db) =>
            {
                if (db.UserDatas == null)
                {
                    return Results.NotFound();
                }
                var userData = await db.UserDatas.FindAsync(id);
                if (userData == null)
                {
                    return Results.NotFound();
                }

                db.UserDatas.Remove(userData);
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("DeleteUserData")
            .WithOpenApi();
        }
    }
}
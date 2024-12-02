using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Extensions
{
    public static class IdentityEndpoints
    {
        public static void MapIdentityEndpoints(this IEndpointRouteBuilder endpoints)
        {
            // User endpoints
            endpoints.MapGet("/identity/users", async (UserManager<IdentityUser> userManager) =>
            {
                var users = await userManager.Users.ToListAsync();
                return Results.Ok(users);
            })
            .WithName("GetIdentityUsers")
            .WithOpenApi();

            endpoints.MapGet("/identity/users/{id}", async (string id, UserManager<IdentityUser> userManager) =>
            {
                var user = await userManager.FindByIdAsync(id);
                return user != null ? Results.Ok(user) : Results.NotFound();
            })
            .WithName("GetIdentityUserById")
            .WithOpenApi();

            endpoints.MapPost("/identity/users", async (IdentityUser user, UserManager<IdentityUser> userManager) =>
            {
                var result = await userManager.CreateAsync(user, "Password123!");
                return result.Succeeded ? Results.Created($"/identity/users/{user.Id}", user) : Results.BadRequest(result.Errors);
            })
            .WithName("CreateIdentityUser")
            .WithOpenApi();

            endpoints.MapPut("/identity/users/{id}", async (string id, IdentityUser updatedUser, UserManager<IdentityUser> userManager) =>
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return Results.NotFound();
                }

                user.UserName = updatedUser.UserName;
                user.Email = updatedUser.Email;

                var result = await userManager.UpdateAsync(user);
                return result.Succeeded ? Results.NoContent() : Results.BadRequest(result.Errors);
            })
            .WithName("UpdateIdentityUser")
            .WithOpenApi();

            endpoints.MapDelete("/identity/users/{id}", async (string id, UserManager<IdentityUser> userManager) =>
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return Results.NotFound();
                }

                var result = await userManager.DeleteAsync(user);
                return result.Succeeded ? Results.NoContent() : Results.BadRequest(result.Errors);
            })
            .WithName("DeleteIdentityUser")
            .WithOpenApi();
        }
    }
}
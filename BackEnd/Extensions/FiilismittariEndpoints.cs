using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BackEnd.Extensions
{
    public static class FiilismittariEndpoints
    {
        public static void MapFiilismittariEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/fiilismittari", async context =>
            {
                await context.Response.WriteAsync("Fiilismittari endpoint");
            })
            .WithName("GetFiilismittari")
            .WithOpenApi();
        }
    }
}
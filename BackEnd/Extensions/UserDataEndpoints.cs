using BackEnd.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

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
                var currentMonthTable = $"UserData_{DateTime.Now:yyyy_MM}";
                var sql = $"SELECT * FROM {currentMonthTable}";
                var userData = db.UserDatas != null ? await db.UserDatas.FromSqlRaw(sql).ToListAsync() : null;
                return userData != null ? Results.Ok(userData) : Results.NotFound();
            })
            .WithName("GetUserDatas")
            .WithOpenApi();

            endpoints.MapGet("/userdatas/{id}", async (int id, BackendDbContext db) =>
            {
                var currentMonthTable = $"UserData_{DateTime.Now:yyyy_MM}";
                var sql = $"SELECT * FROM {currentMonthTable} WHERE Id = {{0}}";
                var userData = db.UserDatas != null ? await db.UserDatas.FromSqlRaw(sql, id).FirstOrDefaultAsync() : null;
                return userData != null ? Results.Ok(userData) : Results.NotFound();
            })
            .WithName("GetUserDataById")
            .WithOpenApi();
        }
    }
}
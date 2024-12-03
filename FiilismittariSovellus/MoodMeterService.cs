using System;
using System.Threading.Tasks;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace BackEnd.Services
{
    public class MoodMeterService
    {
        private readonly BackendDbContext _context;

        public MoodMeterService(BackendDbContext context)
        {
            _context = context;
        }

        public async Task SaveMoodValueAsync(string userId, int value)
        {
            var tableName = $"UserData_{DateTime.Now:yyyy_MM}";
            _context.EnsureMonthlyTableExists(tableName);

            var sql = $@"
                INSERT INTO {tableName} (UserId, Date, Value)
                VALUES (@userId, @date, @value)";
            await _context.Database.ExecuteSqlRawAsync(sql, new[]
            {
                new SqliteParameter("@userId", userId),
                new SqliteParameter("@date", DateTime.Now.ToString("yyyy-MM-dd")),
                new SqliteParameter("@value", value)
            });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services
{
    public class MoodDataService
    {
        private readonly BackendDbContext _context;

        public MoodDataService(BackendDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserData>> GetMoodDataForUser(string userId)
        {
            return await _context.UserDatas
                .Where(ud => ud.UserId == userId)
                .OrderBy(ud => ud.Date)
                .ToListAsync();
        }
        public async Task<UserData?> GetTodayMoodData(string userId)
        {
            var today = DateTime.Now.Date;
            return await _context.UserDatas
                .FirstOrDefaultAsync(ud => ud.UserId == userId && ud.Date == today);
        }

        public async Task SaveMoodValue(string userId, int value)
        {
            var today = DateTime.Now.Date;
            var year = today.Year;
            var month = today.Month;
            var userData = new UserData
            {
                UserId = userId,
                Date = today,
                Value = value,
                Year = year,
                Month = month
            };

            _context.UserDatas.Add(userData);
            await _context.SaveChangesAsync();
        }
    }
}
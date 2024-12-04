using System;
using System.Linq;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services
{
    public class MoodMeterService
    {
        private readonly BackendDbContext _context;

        public MoodMeterService(BackendDbContext context)
        {
            _context = context;
        }

        public UserData? GetTodayMoodData(string userId)
        {
            var today = DateTime.Now.Date;
            return _context.UserDatas
                .FirstOrDefault(ud => ud.UserId == userId && ud.Date == today);
        }

        public bool MoodValueExists(string userId, DateTime date)
        {
            return _context.UserDatas.Any(ud => ud.UserId == userId && ud.Date == date);
        }

        public void SaveMoodValue(string userId, int value, DateTime date)
        {
            var existingMood = _context.UserDatas.FirstOrDefault(ud => ud.UserId == userId && ud.Date == date);

            if (existingMood != null)
            {
                existingMood.Value = value;
                _context.UserDatas.Update(existingMood);
            }
            else
            {
                var userData = new UserData
                {
                    UserId = userId,
                    Date = date,
                    Value = value,
                    Year = date.Year,
                    Month = date.Month
                };
                _context.UserDatas.Add(userData);
            }

            _context.SaveChanges();
        }
    }
}
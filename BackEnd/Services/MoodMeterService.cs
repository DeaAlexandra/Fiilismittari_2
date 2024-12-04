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
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time"); // Esimerkki: Suomen aikavyöhyke
            var today = TimeZoneInfo.ConvertTime(DateTime.Now, timeZone).Date;
            return _context.UserDatas
                .FirstOrDefault(ud => ud.UserId == userId && ud.Date == today);
        }

        public bool MoodValueExists(string userId, DateTime date)
        {
            return _context.UserDatas.Any(ud => ud.UserId == userId && ud.Date == date);
        }

        public void SaveMoodValue(string userId, int value)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time"); // Esimerkki: Suomen aikavyöhyke
            var today = TimeZoneInfo.ConvertTime(DateTime.Now, timeZone).Date;
            var existingMood = _context.UserDatas.FirstOrDefault(ud => ud.UserId == userId && ud.Date == today);

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
                    Date = today,
                    Value = value,
                    Year = today.Year,
                    Month = today.Month
                };
                _context.UserDatas.Add(userData);
            }

            _context.SaveChanges();
        }
    }
}
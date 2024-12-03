using System;
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

        public void SaveMoodValue(string userId, int value)
        {
            var today = DateTime.Now.Date;
            var userData = new UserData
            {
                UserId = userId,
                Date = today,
                Value = value
            };

            _context.UserDatas.Add(userData);
            _context.SaveChanges();
        }
    }
}
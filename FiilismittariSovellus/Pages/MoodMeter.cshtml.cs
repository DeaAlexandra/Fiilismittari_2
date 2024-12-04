using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using BackEnd.Models;
using BackEnd.Services;
using System;
using System.Linq;

namespace FiilismittariSovellus.Pages
{
    [Authorize]
    public class MoodMeterModel : PageModel
    {
        private readonly MoodMeterService _moodMeterService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly BackendDbContext _context;
        private readonly ILogger<MoodMeterModel> _logger;

        public MoodMeterModel(MoodMeterService moodMeterService, UserManager<IdentityUser> userManager, BackendDbContext context, ILogger<MoodMeterModel> logger)
        {
            _moodMeterService = moodMeterService;
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string LastName { get; set; }

        public bool IsUserInfoComplete { get; set; }
        public bool ShowNameForm { get; set; }
        public int? TodayMoodValue { get; set; }

        public void OnGet()
        {
            var userId = _userManager.GetUserId(User);
            _logger.LogInformation("Fetching user data for UserId: {UserId}", userId);
            var user = _context.Users.Find(userId);

            if (user != null)
            {
                FirstName = user.FirstName;
                IsUserInfoComplete = !string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName);
                _logger.LogInformation("User data retrieved: {User}", user);

                // Hae päivän arvo suoraan tietokannasta
                var moodData = _moodMeterService.GetTodayMoodData(userId);
                TodayMoodValue = moodData?.Value;
            }
            else
            {
                IsUserInfoComplete = false;
                _logger.LogWarning("User data not found for UserId: {UserId}", userId);
            }
        }

        public IActionResult OnPostSaveUserInfo()
        {
            var userId = _userManager.GetUserId(User);
            _logger.LogInformation("Saving user data for UserId: {UserId}", userId);
            var user = _context.Users.Find(userId);

            if (user == null)
            {
                user = new User { Id = userId, FirstName = FirstName, LastName = LastName };
                _context.Users.Add(user);
                _logger.LogInformation("New user created: {User}", user);
            }
            else
            {
                user.FirstName = FirstName;
                user.LastName = LastName;
                _context.Users.Update(user);
                _logger.LogInformation("User data updated: {User}", user);
            }

            _context.SaveChanges();
            return RedirectToPage();
        }

        public IActionResult OnPostSaveMood(int value, string selectedDate)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                _logger.LogWarning("User ID not found.");
                return BadRequest("User ID not found.");
            }

            var date = DateTime.Parse(selectedDate);
            if (date > DateTime.Now.Date)
            {
                _logger.LogWarning("Cannot save mood value for a future date.");
                return BadRequest("Cannot save mood value for a future date.");
            }

            if (_moodMeterService.MoodValueExists(userId, date))
            {
                // Kysy käyttäjältä, haluaako hän ylikirjoittaa arvon
                TempData["OverwritePrompt"] = true;
                TempData["MoodValue"] = value;
                TempData["SelectedDate"] = date;
                return RedirectToPage();
            }

            _logger.LogInformation("Saving mood value for UserId: {UserId}, Value: {Value}, Date: {Date}", userId, value, date);
            _moodMeterService.SaveMoodValue(userId, value, date);
            return RedirectToPage();
        }

        public IActionResult OnPostOverwriteMood()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                _logger.LogWarning("User ID not found.");
                return BadRequest("User ID not found.");
            }

            var value = (int)TempData["MoodValue"];
            var selectedDate = (DateTime)TempData["SelectedDate"];
            _logger.LogInformation("Overwriting mood value for UserId: {UserId}, Value: {Value}, Date: {Date}", userId, value, selectedDate);
            _moodMeterService.SaveMoodValue(userId, value, selectedDate);
            return RedirectToPage();
        }

        public IActionResult OnPostShowNameForm()
        {
            ShowNameForm = true;
            return Page();
        }
    }
}
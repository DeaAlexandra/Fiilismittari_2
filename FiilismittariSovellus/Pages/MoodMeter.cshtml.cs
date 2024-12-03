using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BackEnd.Services;
using BackEnd.Models;
using System.Threading.Tasks;

namespace FiilismittariSovellus.Pages
{
    [Authorize]
    public class MoodMeterModel : PageModel
    {
        private readonly MoodMeterService _moodMeterService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly BackendDbContext _context;

        public MoodMeterModel(MoodMeterService moodMeterService, UserManager<IdentityUser> userManager, BackendDbContext context)
        {
            _moodMeterService = moodMeterService;
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string LastName { get; set; }

        public bool IsUserInfoComplete { get; set; }

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _context.Users.FindAsync(userId);

            if (user != null)
            {
                FirstName = user.FirstName;
                IsUserInfoComplete = !string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName);
            }
            else
            {
                IsUserInfoComplete = false;
            }
        }

        public async Task<IActionResult> OnPostSaveUserInfoAsync()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                user = new User { Id = userId, FirstName = FirstName, LastName = LastName };
                _context.Users.Add(user);
            }
            else
            {
                user.FirstName = FirstName;
                user.LastName = LastName;
                _context.Users.Update(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSaveMoodAsync(int value)
        {
            var userId = _userManager.GetUserId(User);
            await _moodMeterService.SaveMoodValueAsync(userId, value);
            return RedirectToPage();
        }
    }
}
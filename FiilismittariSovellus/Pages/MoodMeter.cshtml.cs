using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FiilismittariSovellus.Pages
{
    [Authorize]
    public class MoodMeterModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
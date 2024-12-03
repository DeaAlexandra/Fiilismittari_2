using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BackEnd.Models;
using BackEnd.Services;
using System;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MoodDataController : ControllerBase
    {
        private readonly MoodDataService _moodDataService;
        private readonly ILogger<MoodDataController> _logger;

        public MoodDataController(MoodDataService moodDataService, ILogger<MoodDataController> logger)
        {
            _moodDataService = moodDataService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetMoodData()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                _logger.LogWarning("User ID not found.");
                return BadRequest("User ID not found.");
            }

            var moodData = await _moodDataService.GetMoodDataForUser(userId);

            if (moodData == null || moodData.Count == 0)
            {
                _logger.LogWarning("No mood data found for UserId: {UserId}", userId);
                return NotFound("User data not found.");
            }

            _logger.LogInformation("Mood data retrieved for UserId: {UserId}", userId);
            return Ok(moodData);
        }

        [HttpPost]
        public async Task<IActionResult> SaveMoodData([FromBody] UserData userData)
        {
            if (userData == null)
            {
                _logger.LogWarning("Invalid user data.");
                return BadRequest("Invalid user data.");
            }

            await _moodDataService.SaveMoodValue(userData.UserId, userData.Value);
            _logger.LogInformation("Mood data saved for UserId: {UserId} on {Date}: {MoodValue}", userData.UserId, userData.Date, userData.Value);
            return Ok(userData);
        }
    }
}
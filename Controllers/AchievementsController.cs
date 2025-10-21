using Microsoft.AspNetCore.Mvc;
using CalorieTracker.Context;
using CalorieTracker.Models;

namespace CalorieTracker.Controllers
{
    [Route("api/achievements")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class AchievementsController : Controller
    {
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public ActionResult GetUserAchievements()
        {
            try
            {
                var userId = GetUserIdFromToken();
                if (userId == null) return Unauthorized("Требуется авторизация");

                using (var context = new CalorieContext())
                {
                    var achievements = context.Achievements
                        .Where(x => x.UserId == userId)
                        .OrderByDescending(x => x.EarnedAt)
                        .ToList();

                    return Ok(achievements);
                }
            }
            catch (Exception exp)
            {
                return StatusCode(500, "Ошибка сервера");
            }
        }

        private int? GetUserIdFromToken()
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length);
                try
                {
                    var parts = token.Split('_');
                    if (parts.Length >= 3 && int.TryParse(parts[2], out int userId))
                        return userId;
                }
                catch { }
            }
            return null;
        }
    }
}
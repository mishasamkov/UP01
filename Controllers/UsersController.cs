using Microsoft.AspNetCore.Mvc;
using CalorieTracker.Context;
using CalorieTracker.Models;

namespace CalorieTracker.Controllers
{
    [Route("api/users")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class UsersController : Controller
    {
        [HttpGet("me")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public ActionResult GetCurrentUser()
        {
            try
            {
                var userId = GetUserIdFromToken();
                if (userId == null) return Unauthorized("Требуется авторизация");

                using (var context = new CalorieContext())
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == userId);
                    if (user == null) return NotFound("Пользователь не найден");

                    return Ok(new
                    {
                        id = user.Id,
                        username = user.Username,
                        email = user.Email,
                        dailyCalorieGoal = user.DailyCalorieGoal,
                        createdAt = user.CreatedAt
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Ошибка сервера");
            }
        }

        private int? GetUserIdFromToken()
        {
            // Упрощенная реализация - в реальном приложении нужно парсить JWT
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length);
                // Парсинг токена и получение userId
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

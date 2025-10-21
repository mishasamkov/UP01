using Microsoft.AspNetCore.Mvc;
using CalorieTracker.Context;
using CalorieTracker.Models;
using CalorieTracker.DTOs;

namespace CalorieTracker.Controllers
{
    [Route("api/goals")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class GoalsController : Controller
    {
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public ActionResult GetUserGoals()
        {
            try
            {
                var userId = GetUserIdFromToken();
                if (userId == null) return Unauthorized("Требуется авторизация");

                using (var context = new CalorieContext())
                {
                    var goals = context.Goals
                        .Where(x => x.UserId == userId)
                        .OrderByDescending(x => x.StartDate)
                        .ToList();

                    return Ok(goals);
                }
            }
            catch (Exception exp)
            {
                return StatusCode(500, "Ошибка сервера");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public ActionResult CreateGoal([FromBody] GoalRequest request)
        {
            try
            {
                var userId = GetUserIdFromToken();
                if (userId == null) return Unauthorized("Требуется авторизация");

                using (var context = new CalorieContext())
                {
                    var goal = new Goal
                    {
                        UserId = userId.Value,
                        TargetType = request.TargetType,
                        TargetValue = request.TargetValue,
                        StartDate = request.StartDate,
                        EndDate = request.EndDate,
                        IsCompleted = false
                    };

                    context.Goals.Add(goal);
                    context.SaveChanges();
                    return StatusCode(201);
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
using Microsoft.AspNetCore.Mvc;
using CalorieTracker.Context;
using CalorieTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace CalorieTracker.Controllers
{
    [Route("api/stats")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class StatsController : Controller
    {
        private readonly StatsContext _context;

        public StatsController(StatsContext context)
        {
            _context = context;
        }

        [HttpGet("daily")]
        public ActionResult GetDailyStats()
        {
            var userId = GetUserIdFromToken();
            if (userId == null) return Unauthorized();

            var today = DateTime.Today;
            var meals = _context.Meals
                .Where(x => x.UserId == userId && x.ConsumedAt.Date == today)
                .Include(x => x.Product)
                .ToList();

            // ... остальная логика
            return Ok(/* результат */);
        }
    }
}
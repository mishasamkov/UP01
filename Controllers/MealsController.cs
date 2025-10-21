using Microsoft.AspNetCore.Mvc;
using CalorieTracker.Context;
using CalorieTracker.Models;
using CalorieTracker.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CalorieTracker.Controllers
{
    [Route("api/meals")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class MealsController : Controller
    {
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public ActionResult GetUserMeals()
        {
            try
            {
                var userId = GetUserIdFromToken();
                if (userId == null) return Unauthorized("Требуется авторизация");

                using (var context = new CalorieContext())
                {
                    var meals = context.Meals
                        .Where(x => x.UserId == userId)
                        .Include(x => x.Product)
                        .OrderByDescending(x => x.ConsumedAt)
                        .ToList();

                    var result = meals.Select(m => new {
                        id = m.Id,
                        productId = m.ProductId,
                        productName = m.Product.Name,
                        weightGrams = m.WeightGrams,
                        calories = Math.Round((m.Product.Calories * m.WeightGrams) / 100, 2),
                        consumedAt = m.ConsumedAt
                    });

                    return Ok(result);
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
        public ActionResult AddMeal([FromBody] MealRequest request)
        {
            try
            {
                var userId = GetUserIdFromToken();
                if (userId == null) return Unauthorized("Требуется авторизация");

                using (var context = new CalorieContext())
                {
                    var meal = new Meal
                    {
                        UserId = userId.Value,
                        ProductId = request.ProductId,
                        WeightGrams = request.WeightGrams,
                        ConsumedAt = DateTime.Now
                    };

                    context.Meals.Add(meal);
                    context.SaveChanges();
                    return StatusCode(201);
                }
            }
            catch (Exception exp)
            {
                return StatusCode(500, "Ошибка сервера");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult DeleteMeal(int id)
        {
            try
            {
                var userId = GetUserIdFromToken();
                if (userId == null) return Unauthorized("Требуется авторизация");

                using (var context = new CalorieContext())
                {
                    var meal = context.Meals.FirstOrDefault(x => x.Id == id && x.UserId == userId);
                    if (meal == null) return NotFound("Прием пищи не найден");

                    context.Meals.Remove(meal);
                    context.SaveChanges();
                    return StatusCode(204);
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

using Microsoft.AspNetCore.Mvc;
using CalorieTracker.Context;
using CalorieTracker.Models;
using CalorieTracker.DTOs;

namespace CalorieTracker.Controllers
{
    [Route("api/auth")]
    [ApiExplorerSettings(GroupName = "v2")]
    public class AuthController : Controller
    {
        private readonly AuthContext _context;

        public AuthController(AuthContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] UserRegisterRequest request)
        {
            // Используем _context вместо создания нового
            if (_context.Users.Any(x => x.Email == request.Email))
                return BadRequest("Пользователь с таким email уже существует");

            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                DailyCalorieGoal = 2000
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return StatusCode(201, new
            {
                id = newUser.Id,
                username = newUser.Username,
                email = newUser.Email
            });
        }
    }
}
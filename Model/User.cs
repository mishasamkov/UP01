using System.ComponentModel.DataAnnotations;

namespace CalorieTracker.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public int DailyCalorieGoal { get; set; } = 2000;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

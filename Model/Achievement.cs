using System.ComponentModel.DataAnnotations;

namespace CalorieTracker.Models
{
    public class Achievement
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime EarnedAt { get; set; } = DateTime.Now;
    }
}

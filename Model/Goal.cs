namespace CalorieTracker.Models
{
    public class Goal
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string TargetType { get; set; } // "weight_loss", "weight_gain", "maintain"

        public decimal TargetValue { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}

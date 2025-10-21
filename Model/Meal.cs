namespace CalorieTracker.Models
{
    public class Meal
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public decimal WeightGrams { get; set; }

        public DateTime ConsumedAt { get; set; } = DateTime.Now;
    }
}
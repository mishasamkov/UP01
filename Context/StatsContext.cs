using Microsoft.EntityFrameworkCore;
using CalorieTracker.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CalorieTracker.Context
{
    public class StatsContext : DbContext
    {
        public StatsContext(DbContextOptions<StatsContext> options) : base(options)
        {
        }

        public DbSet<Meal> Meals { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=127.0.0.1;uid=root;pwd=;database=CalorieTracker",
                    new MySqlServerVersion(new Version(8, 0, 11)));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meal>()
                .HasOne(m => m.Product)
                .WithMany()
                .HasForeignKey(m => m.ProductId);
        }
    }
}
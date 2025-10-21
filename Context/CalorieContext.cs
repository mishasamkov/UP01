using Microsoft.EntityFrameworkCore;
using CalorieTracker.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CalorieTracker.Context
{
    public class CalorieContext : DbContext
    {
        public CalorieContext(DbContextOptions<CalorieContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Achievement> Achievements { get; set; }

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
            // Конфигурация связей
            modelBuilder.Entity<Meal>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId);

            modelBuilder.Entity<Meal>()
                .HasOne(m => m.Product)
                .WithMany()
                .HasForeignKey(m => m.ProductId);

            modelBuilder.Entity<Goal>()
                .HasOne(g => g.User)
                .WithMany()
                .HasForeignKey(g => g.UserId);

            modelBuilder.Entity<Achievement>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId);
        }
    }
}
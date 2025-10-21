using Microsoft.EntityFrameworkCore;
using CalorieTracker.Models;
using System.Collections.Generic;

namespace CalorieTracker.Context
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=127.0.0.1;uid=root;pwd=;database=CalorieTracker",
                    new MySqlServerVersion(new Version(8, 0, 11)));
            }
        }
    }
}
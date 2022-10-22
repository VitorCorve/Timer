using Microsoft.EntityFrameworkCore;

using System.Configuration;

namespace TimerContext.Models.Context
{
    public class TimerDbContext : DbContext
    {
        public TimerDbContext(DbContextOptions options) : base(options)
        {
        }

        protected TimerDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Statistics> Statistics { get; set; }
        public DbSet<Rank> Ranks { get; set; }
    }
}

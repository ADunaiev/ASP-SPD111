using Microsoft.EntityFrameworkCore;

namespace ASP_SPD111.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Entities.User> Users { get; set; }
        public DbSet<Entities.HomeWotkUser> HomeWotkUsers { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ASP_SPD111");
        }
    }
}

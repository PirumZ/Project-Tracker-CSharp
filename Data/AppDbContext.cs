using Microsoft.EntityFrameworkCore;
using Project_Tracker_C_.Models;

namespace Project_Tracker_C_.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) 
        { 
        }

        public DbSet<TaskItem> Tasks { get; set; }
    }
}

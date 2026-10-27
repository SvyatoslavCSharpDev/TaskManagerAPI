using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> _options) : base(_options) { }

        public DbSet<TaskItem> TasksItems { get; set; }
    }
}

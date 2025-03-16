using Microsoft.EntityFrameworkCore;
using task_back.Models;
//using TaskManagerAPI.Models;

namespace task_back.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }
        public DbSet<TaskItem> Tasks { get; set; }
    }
}
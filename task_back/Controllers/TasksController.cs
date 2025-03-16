using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task_back.Data;
using task_back.Models;

namespace task_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public TasksController(TaskDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks([FromQuery] string? status)
        {
            IQueryable<TaskItem> query = _context.Tasks;

            if (!string.IsNullOrEmpty(status))
            {
                switch (status.ToLower())
                {
                    case "completed":
                        query = query.Where(t => t.IsCompleted);
                        break;
                    case "pending":
                        query = query.Where(t => !t.IsCompleted);
                        break;
                    case "all":
                    default:
                        break;
                }
            }

            return await query.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItem task)
        {
            if (id != task.Id)
                return BadRequest();

            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return NotFound();

            if (!task.IsCompleted)
            {
                return BadRequest(new { message = "No se puede eliminar una tarea que aún está pendiente." });
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateTaskStatus(int id, UpdateTaskStatusRequest request)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            task.IsCompleted = request.IsCompleted;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        public class UpdateTaskStatusRequest
        {
            public bool IsCompleted { get; set; }
        }
    }
}

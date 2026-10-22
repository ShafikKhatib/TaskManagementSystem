using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TaskItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/taskitem/{employeeId}
        [HttpGet("{employeeId}/getEmployeeTask")]
        public async Task<IActionResult> GetTasksByEmployee(int employeeId)
        {
            try
            {
                var tasks = await _context.TaskItems
                    .Where(t => t.EmployeeId == employeeId)
                    .Include(t => t.TaskNotes)
                    .Include(t => t.Employee)
                    .ToListAsync();

                if (tasks == null || tasks.Count == 0)
                    return NotFound("Task not found for given employee");

                var response = tasks.Select(task => new
                {
                    task.TaskId,
                    task.Title,
                    task.Description,
                    task.DueDate,
                    task.IsCompleted,
                    Employee = task.Employee == null ? null : new
                    {
                        task.Employee.EmployeeId,
                        task.Employee.Name
                    },
                    Notes = task.TaskNotes == null ? null : task.TaskNotes.Select(note => new
                    {
                        note.NoteId,
                        note.Content,
                        note.CreatedDate
                    })
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving tasks.", error = ex.Message });
            }
        }

        // POST: api/taskitem/CreateTask
        [HttpPost("createTask")]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (task.EmployeeId <= 0)
                    return BadRequest("Invalid EmployeeId");

                var employeeExists = await _context.Employees.AnyAsync(e => e.EmployeeId == task.EmployeeId);
                if (!employeeExists)
                    return NotFound("Employee not found");

                _context.TaskItems.Add(task);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Task created successfully",
                    task = task
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the task.", error = ex.Message });
            }
        }

        // PUT: api/taskitem/{id}/UpdateTask
        [HttpPut("{id}/updateTask")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem task)
        {
            try
            {
                if (id != task.TaskId)
                    return BadRequest("Task ID mismatch");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingTask = await _context.TaskItems.FindAsync(id);
                if (existingTask == null)
                    return NotFound("Task not found");

                _context.Entry(existingTask).State = EntityState.Detached;
                _context.Entry(task).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Task updated successfully",
                    task = task
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the task.", error = ex.Message });
            }
        }

        // DELETE: api/taskitem/{id}/DeleteTask
        [HttpDelete("{id}/deleteTask")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var task = await _context.TaskItems.FindAsync(id);
                if (task == null)
                    return NotFound("Task not found");

                _context.TaskItems.Remove(task);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Task deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the task.", error = ex.Message });
            }
        }

        [HttpPost("{id}/markComplete")]
        public async Task<IActionResult> MarkTaskAsCompleted(int id)
        {
            try
            {
                var task = await _context.TaskItems.FindAsync(id);
                if (task == null)
                    return NotFound("Task not found");

                if (!task.IsCompleted)
                {
                    task.IsCompleted = true;
                    _context.TaskItems.Update(task);
                    await _context.SaveChangesAsync();

                    return Ok(new { message = "Task marked as completed" });
                }
                else
                {
                    return BadRequest(new { message = "Task is already marked as completed" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while marking the task as completed.", error = ex.Message });
            }
        }

        [HttpPost("{id}/unmarkComplete")]
        public async Task<IActionResult> MarkTaskAsInCompleted(int id)
        {
            try
            {
                var task = await _context.TaskItems.FindAsync(id);
                if (task == null)
                    return NotFound("Task not found");

                if (task.IsCompleted)
                {
                    task.IsCompleted = false;
                    _context.TaskItems.Update(task);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Task completed unmarked" });
                }
                else
                {
                    return BadRequest(new { message = "Task is not marked as completed" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while unmarking the task.", error = ex.Message });
            }
        }
    }
}

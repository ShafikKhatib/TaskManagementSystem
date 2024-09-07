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
            // Include Notes when retrieving TaskItems
            var tasks = await _context.TaskItems
                .Where(t => t.EmployeeId == employeeId)
                .Include(t => t.TaskNotes) // Eagerly load Notes
                .ToListAsync();

            if (tasks == null || tasks.Count == 0)
                return NotFound("Task not found for given employee");

            // Shape the response to include Notes content
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

        // POST: api/taskitem/CreateTask
        [HttpPost("createTask")]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Task created successfully",
                task = task
            });
        }

        // PUT: api/taskitem/{id}/UpdateTask
        [HttpPut("{id}/updateTask")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem task)
        {
            if (id != task.TaskId)
                return BadRequest("Task not found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Task updated successfully",
                task = task
            });
        }

        // DELETE: api/taskitem/{id}/DeleteTask
        [HttpDelete("{id}/deleteTask")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
                return NotFound("Task not found");

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();

            return Ok(new {message = "Task deleted successfully"});
        }

        [HttpPost("{id}/markComplete")]
        public async Task<IActionResult> MarkTaskAsCompleted(int id)
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

        [HttpPost("{id}/unmarkComplete")]
        public async Task<IActionResult> MarkTaskAsInCompleted(int id)
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

        // POST: api/taskitem/{taskId}/addNote
        [HttpPost("{taskId}/addNote")]
        public async Task<IActionResult> AddNoteToTask(int taskId, [FromBody] TaskNotes note)
        {
            // Check if the task exists
            var task = await _context.TaskItems.FindAsync(taskId);
            if (task == null)
                return NotFound("Task not found");

            // Add the note to the task
            note.TaskId = taskId;
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Note added successfully", note });
        }

        // GET: api/taskitem/{taskId}/getNotes
        [HttpGet("{taskId}/getNotes")]
        public async Task<IActionResult> GetNotesForTask(int taskId)
        {
            var task = await _context.TaskItems
                .Include(t => t.TaskNotes) // Include related Notes
                .FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null)
                return NotFound("Task not found");

            var notes = task.TaskNotes.Select(note => new
            {
                note.NoteId,
                note.Content,
                note.CreatedDate
            });

            return Ok(notes);
        }

        // DELETE: api/taskitem/{taskId}/deleteNotes/{noteId}
        [HttpDelete("{taskId}/deleteNotes/{noteId}")]
        public async Task<IActionResult> DeleteNoteForTask(int taskId, int noteId)
        {
            var task = await _context.TaskItems
                .Include(t => t.TaskNotes) // Include related Notes
                .FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null)
                return NotFound("Task not found");

            var note = task.TaskNotes.FirstOrDefault(n => n.NoteId == noteId);
            if (note == null)
                return NotFound("Note not found");

            _context.Notes.Remove(note); // Remove the note
            await _context.SaveChangesAsync();

            return Ok(new { message = "Note deleted successfully" });
        }


        [HttpPost("{taskId}/uploadDocument")]
        public async Task<IActionResult> UploadDocument(int taskId, [FromForm] IFormFile document)
        {
            if (document == null || document.Length == 0)
                return BadRequest("Invalid file.");

            var task = await _context.TaskItems.FindAsync(taskId);
            if (task == null)
                return NotFound("Task not found");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "documents", document.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await document.CopyToAsync(stream);
            }

            return Ok(new { message = "Document uploaded successfully", path });
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/admin/report/due-tasks
        [HttpGet("report/due-tasks")]
        public async Task<IActionResult> GetTasksDueInAWeek()
        {
            try
            {
                var currentDate = DateTime.Now;
                var dueTasks = await _context.TaskItems
                    .Include(t => t.Employee)
                    .ThenInclude(e => e.Team)
                    .Where(t => t.DueDate <= currentDate.AddDays(7) && !t.IsCompleted)
                    .Select(t => new
                    {
                        t.TaskId,
                        t.Title,
                        t.Description,
                        t.DueDate,
                        t.IsCompleted,
                        t.EmployeeId,
                        EmployeeName = t.Employee != null ? t.Employee.Name : "Unknown",
                        TeamName = t.Employee != null && t.Employee.Team != null ? t.Employee.Team.TeamName : "Unknown"
                    })
                    .ToListAsync();

                return Ok(dueTasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving tasks.", error = ex.Message });
            }
        }

        // GET: api/admin/report/completed-tasks
        [HttpGet("report/completed-tasks")]
        public async Task<IActionResult> GetCompletedTasksForMonth()
        {
            try
            {
                var currentDate = DateTime.Now;
                var completedTasks = await _context.TaskItems
                    .Include(t => t.Employee)
                    .ThenInclude(e => e.Team)
                    .Where(t => t.IsCompleted && t.DueDate.Month == currentDate.Month)
                    .Select(t => new
                    {
                        t.TaskId,
                        t.Title,
                        t.Description,
                        t.DueDate,
                        t.IsCompleted,
                        t.EmployeeId,
                        EmployeeName = t.Employee != null ? t.Employee.Name : "Unknown",
                        TeamName = t.Employee != null && t.Employee.Team != null ? t.Employee.Team.TeamName : "Unknown"
                    })
                    .ToListAsync();

                return Ok(completedTasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving completed tasks.", error = ex.Message });
            }
        }
    }
}

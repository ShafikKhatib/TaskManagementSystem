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
            var currentDate = DateTime.Now;
            var dueTasks = await _context.TaskItems
                .Select(t => new
                {
                    t.TaskId,
                    t.Title,
                    t.Description,
                    t.DueDate,
                    t.IsCompleted,
                    t.EmployeeId,
                    EmployeeName = t.Employee.Name,
                    TeamName = t.Employee.Team.TeamName
                })
                .Where(t => t.DueDate <= currentDate.AddDays(7) && !t.IsCompleted)
                .ToListAsync();

            return Ok(dueTasks);
        }

        // GET: api/admin/report/completed-tasks
        [HttpGet("report/completed-tasks")]
        public async Task<IActionResult> GetCompletedTasksForMonth()
        {
            var currentDate = DateTime.Now;
            var completedTasks = await _context.TaskItems
                .Select(t => new
                {
                    t.TaskId,
                    t.Title,
                    t.Description,
                    t.DueDate,
                    t.IsCompleted,
                    t.EmployeeId,
                    EmployeeName = t.Employee.Name, 
                    TeamName = t.Employee.Team.TeamName 
                })
                .Where(t => t.IsCompleted && t.DueDate.Month == currentDate.Month)
                .ToListAsync();

            return Ok(completedTasks);
        }
    }
}

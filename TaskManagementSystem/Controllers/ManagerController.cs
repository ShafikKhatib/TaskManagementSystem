using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/manager/{managerId}/team-tasks
        [HttpGet("{managerId}/team-tasks")]
        public async Task<IActionResult> GetTeamTasks(int managerId)
        {
            try
            {
                var manager = await _context.Employees
                    .Include(e => e.Team)
                    .FirstOrDefaultAsync(e => e.EmployeeId == managerId && (e.Role == "Manager" || e.Role == "TeamLead"));

                if (manager == null)
                    return NotFound("Manager or TeamLead not found");

                if (manager.TeamId == null)
                    return BadRequest("Manager does not have an assigned team");

                var teamTasks = await _context.TaskItems
                    .Include(t => t.Employee)
                    .ThenInclude(e => e.Team)
                    .Include(t => t.TaskNotes)
                    .Where(t => t.Employee.TeamId == manager.TeamId)
                    .Select(t => new
                    {
                        t.TaskId,
                        t.Title,
                        t.Description,
                        t.DueDate,
                        t.IsCompleted,
                        t.EmployeeId,
                        EmployeeName = t.Employee.Name,
                        TeamName = t.Employee.Team != null ? t.Employee.Team.TeamName : "Unknown",
                        Notes = t.TaskNotes.Select(note => new
                        {
                            note.NoteId,
                            note.Content,
                            note.CreatedDate
                        })
                    })
                    .ToListAsync();

                return Ok(teamTasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving team tasks.", error = ex.Message });
            }
        }
    }
}

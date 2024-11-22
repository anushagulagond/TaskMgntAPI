using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using TaskMgntAPI.Models;

namespace TaskMgntAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTaskReportController : ControllerBase
    {

        private readonly TaskMgntDbContext _context;

        public UserTaskReportController(TaskMgntDbContext context)
        {
            _context = context;
        }

        // Endpoint to fetch user details by user ID
        [HttpGet("AspNetUsers/{userId}")]
        public async Task<IActionResult> GetUserDetails(string userId)
        {
            var user = await _context.AspNetUsers
                .Where(u => u.Id == userId)
                .Select(u => new
                {
                    UserId = u.Id,
                    UserName = u.UserName,
                    Email = u.Email
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { Message = $"User with ID {userId} not found." });
            }

            return Ok(user);
        }

        // Endpoint to fetch all tasks for a specific user
        [HttpGet("TaskTbls/{userId}")]
        public async Task<IActionResult> GetUserTasks(string userId)
        {
            var tasks = await _context.TaskTbls
                .Where(t => t.AssignedToUserId == userId)
                .Select(t => new
                {
                    TaskId = t.TaskId,
                    TaskName = t.TaskName,
                    Status = t.Status
                })
                .ToListAsync();

            if (tasks == null || tasks.Count == 0)
            {
                return NotFound(new { Message = "No tasks found for this user." });
            }

            return Ok(tasks);
        }

        // Endpoint to generate and download user task report as CSV
        [HttpGet("TaskTbls/{userId}/downloadReport")]
        public async Task<IActionResult> DownloadUserReport(string userId)
        {

            // Fetch user tasks for the given userId
            var tasks = await _context.TaskTbls
                .Where(t => t.AssignedToUserId == userId)
                .Select(t => new
                {
                    t.TaskId,
                    t.TaskName,
                    t.Status,
                    t.TaskStartDate,
                    t.TaskEndDate
                })
                .ToListAsync();

            if (tasks == null || tasks.Count == 0)
            {
                return NotFound(new { Message = "No tasks found for this user." });
            }

            // Generate CSV content
            var csvContent = new StringBuilder();
            csvContent.AppendLine("Task ID,Task Name,Status,Start Date,End Date");

            foreach (var task in tasks)
            {
                csvContent.AppendLine($"{task.TaskId},{task.TaskName},{task.Status},{task.TaskStartDate:yyyy-MM-dd},{task.TaskEndDate:yyyy-MM-dd}");
            }

            var reportBytes = Encoding.UTF8.GetBytes(csvContent.ToString());
            var fileName = $"User_{userId}_Task_Report.csv";

            return File(reportBytes, "text/csv", fileName);
        }
    }

}

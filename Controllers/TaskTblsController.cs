using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskMgntAPI.DTO;
using TaskMgntAPI.Models;
using TaskMgntAPI.Services;

namespace TaskMgntAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTblsController : ControllerBase
    {
        private readonly TaskMgntDbContext _context;
        private readonly IEmailService _emailService;
        public TaskTblsController(TaskMgntDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: api/TaskTbls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskTbl>>> GetTaskTbls()
        {
            return await _context.TaskTbls.ToListAsync();
        }

        // GET: api/TaskTbls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskTbl>> GetTaskTbl(int id)
        {
            var taskTbl = await _context.TaskTbls.FindAsync(id);

            if (taskTbl == null)
            {
                return NotFound();
            }

            return taskTbl;
        }

        // PUT: api/TaskTbls/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskTbl(int id, TaskDTO taskDTO)
        {
            TaskTbl taskTbl = new TaskTbl();
            taskTbl.TaskId = taskDTO.TaskId;
            taskTbl.TaskName = taskDTO.TaskName;
            taskTbl.AssignedToUserId = taskDTO.AssignedToUserId;
            taskTbl.ProjectId = taskDTO.ProjectId;
            taskTbl.Priority = taskDTO.Priority;
            taskTbl.Status = taskDTO.Status;
            taskTbl.TaskStartDate = taskDTO.TaskStartDate;
            taskTbl.TaskEndDate = taskDTO.TaskEndDate;
            taskTbl.Description = taskDTO.Description;

            if (id != taskTbl.TaskId)
            {
                return BadRequest();
            }

            _context.Entry(taskTbl).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskTblExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TaskTbls
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskTbl>> PostTaskTbl(TaskDTO taskDTO)
        {
            TaskTbl taskTbl = new TaskTbl();
            taskTbl.TaskId = taskDTO.TaskId;
            taskTbl.TaskName = taskDTO.TaskName;
            taskTbl.AssignedToUserId = taskDTO.AssignedToUserId;
            taskTbl.ProjectId = taskDTO.ProjectId;
            taskTbl.Priority = taskDTO.Priority;
            taskTbl.Status = taskDTO.Status;
            taskTbl.TaskStartDate = taskDTO.TaskStartDate;
            taskTbl.TaskEndDate = taskDTO.TaskEndDate;
            taskTbl.Description = taskDTO.Description;

            _context.TaskTbls.Add(taskTbl);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TaskTblExists(taskTbl.TaskId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }


            return CreatedAtAction("GetTaskTbl", new { id = taskTbl.TaskId }, taskTbl);
        }

        // DELETE: api/TaskTbls/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskTbl(int id)
        {
            var taskTbl = await _context.TaskTbls.FindAsync(id);
            if (taskTbl == null)
            {
                return NotFound();
            }

            _context.TaskTbls.Remove(taskTbl);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskTblExists(int id)
        {
            return _context.TaskTbls.Any(e => e.TaskId == id);
        }




        //[HttpPatch("{id}")]
        //public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] TaskStatusUpdateDTO request)
        //{
        //    if (string.IsNullOrEmpty(request?.Status))
        //    {
        //        return BadRequest("Status is required.");
        //    }

        //    var task = await _context.TaskTbls.FindAsync(id);
        //    if (task == null)
        //    {
        //        return NotFound("Task not found.");
        //    }

        //    task.Status = request.Status;
        //    task.Priority = request.Priority; // Update priority
        //    task.TaskEndDate = (DateTime)request.TaskEndDate; // Update due date

        //    await _context.SaveChangesAsync();

        //    return NoContent(); // 204 No Content response if successful
        //}



        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] TaskStatusUpdateDTO request)
        {
            if (string.IsNullOrEmpty(request?.Status))
            {
                return BadRequest("Status is required.");
            }

            var task = await _context.TaskTbls.FindAsync(id);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            // Update task details
            task.Status = request.Status;
            task.Priority = request.Priority;
            task.TaskEndDate = request.TaskEndDate;

            await _context.SaveChangesAsync();

            // Fetch user email by userId
            string userEmail;
            try
            {
                userEmail = await _emailService.GetUserEmailByIdAsync(task.AssignedToUserId);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }

            var adminEmail = "samalaabhinaya@example.com"; // Replace with admin email
            var subject = $"Task '{task.TaskName}' Status Updated";
            var body = $"Dear Admin,\n\nThe following task has been updated by the user ({userEmail}):\n\n" +
                       $"- Task Name: {task.TaskName}\n" +
                       $"- Status: {task.Status}\n" +
                       $"- Priority: {task.Priority}\n" +
                       $"- End Date: {task.TaskEndDate:yyyy-MM-dd}\n\n" +
                       "Best regards,\nTask Management System";

            try
            {
                await _emailService.SendEmailAsync(adminEmail, subject, body, userEmail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                return StatusCode(500, "Task updated, but failed to send email to the admin.");
            }

            return NoContent(); // Task updated and email sent successfully
        }



        [HttpGet("getProjectDetails/{projectId}")]
        public async Task<IActionResult> GetProjectDetails(int projectId)
        {
            // Fetch the project details
            var project = await _context.Projects
                .Where(p => p.ProjectId == projectId)
                .Select(p => new
                {
                    p.ProjectId,
                    p.ProjectName,
                    p.Description,
                    p.StartDate,
                    p.EndDate,
                    Tasks = _context.TaskTbls
                        .Where(t => t.ProjectId == projectId)
                        .Select(t => new
                        {
                            t.TaskId,
                            t.TaskName,
                            t.Description,
                            t.TaskStartDate,
                            t.TaskEndDate,
                            t.Priority,
                            t.Status
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (project == null)
            {
                return NotFound(new { Message = $"No project found with ID: {projectId}" });
            }

            return Ok(project);
        }

        [HttpGet("downloadProjectReport/{projectId}")]
        public async Task<IActionResult> DownloadProjectReport(int projectId)
        {
            // Fetch the project details
            var project = await _context.Projects
                .Where(p => p.ProjectId == projectId)
                .Select(p => new
                {
                    p.ProjectName,
                    p.Description,
                    p.StartDate,
                    p.EndDate,
                    Tasks = _context.TaskTbls
                        .Where(t => t.ProjectId == projectId)
                        .Select(t => new
                        {
                            t.TaskName,
                            t.Description,
                            t.TaskStartDate,
                            t.TaskEndDate,
                            t.Priority,
                            t.Status
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (project == null)
            {
                return NotFound($"No project found with ID: {projectId}");
            }

            // Generate CSV content
            var reportContent = new StringBuilder();
            reportContent.AppendLine($"Project Name: {project.ProjectName}");
            reportContent.AppendLine($"Description: {project.Description}");
            reportContent.AppendLine($"Start Date: {project.StartDate:yyyy-MM-dd}");
            reportContent.AppendLine($"End Date: {project.EndDate:yyyy-MM-dd}");
            reportContent.AppendLine("Tasks:");
            reportContent.AppendLine("Task Name,Description,Start Date,End Date,Priority,Status");

            foreach (var task in project.Tasks)
            {
                reportContent.AppendLine($"{task.TaskName},{task.Description},{task.TaskStartDate:yyyy-MM-dd},{task.TaskEndDate:yyyy-MM-dd},{task.Priority},{task.Status}");
            }

            var reportBytes = Encoding.UTF8.GetBytes(reportContent.ToString());
            var fileName = $"Project_{projectId}_Report.csv";

            return File(reportBytes, "text/csv", fileName);
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

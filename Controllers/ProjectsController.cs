using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskMgntAPI.DTO;
using TaskMgntAPI.Models;

namespace TaskMgntAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly TaskMgntDbContext _context;

        public ProjectsController(TaskMgntDbContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProject()
        {
            return await _context.Projects.ToListAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, ProjectDTO projectDTO)
        {
            Project project = new Project();
            project.ProjectId = projectDTO.ProjectId;
            project.ProjectName = projectDTO.ProjectName;
            project.StartDate = projectDTO.StartDate;
            project.EndDate = projectDTO.EndDate;
            project.Status = projectDTO.Status;
            project.Description = projectDTO.Description;

            if (id != project.ProjectId)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(ProjectDTO projectDTO)
        {
            Project project = new Project();
            project.ProjectId = projectDTO.ProjectId;
            project.ProjectName = projectDTO.ProjectName;
            project.StartDate = projectDTO.StartDate;
            project.EndDate = projectDTO.EndDate;
            project.Status = projectDTO.Status;
            project.Description = projectDTO.Description;

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.ProjectId }, project);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }




        // Generate and download report for all projects
        [HttpGet("report")]
        public ActionResult GenerateReport()
        {
            try
            {
                var projectDetails = (from p in _context.Projects
                                      join t in _context.TaskTbls on p.ProjectId equals t.ProjectId
                                      select new
                                      {
                                          p.ProjectId,
                                          p.ProjectName,
                                          p.Description,
                                          p.StartDate,
                                          p.EndDate,
                                          Tasks = _context.TaskTbls
                                              .Where(task => task.ProjectId == p.ProjectId)
                                              .Select(task => new
                                              {
                                                  task.TaskId,
                                                  task.TaskName,
                                                  task.Description,
                                                  task.TaskStartDate,
                                                  task.TaskEndDate,
                                                  task.Priority,
                                                  task.Status
                                              }).ToList()
                                      }).ToList(); // Fetch all projects and their tasks

                if (projectDetails == null || !projectDetails.Any())
                {
                    return NotFound("No projects found.");
                }

                // Build the report content
                var reportContent = new StringBuilder();
                reportContent.AppendLine("Project Report");
                reportContent.AppendLine("====================================");

                foreach (var project in projectDetails)
                {
                    reportContent.AppendLine($"Project Name: {project.ProjectName}");
                    reportContent.AppendLine($"Description: {project.Description}");
                    reportContent.AppendLine($"Start Date: {project.StartDate:yyyy-MM-dd}");
                    reportContent.AppendLine($"End Date: {project.EndDate:yyyy-MM-dd}");
                    reportContent.AppendLine();
                    reportContent.AppendLine("Tasks:");
                    reportContent.AppendLine("Task Name,Description,Start Date,End Date,Priority,Status");

                    foreach (var task in project.Tasks)
                    {
                        reportContent.AppendLine($"{task.TaskName},{task.Description},{task.TaskStartDate:yyyy-MM-dd},{task.TaskEndDate:yyyy-MM-dd},{task.Priority},{task.Status}");
                    }
                    reportContent.AppendLine();
                }

                // Convert the report content to a byte array for download
                var reportBytes = Encoding.UTF8.GetBytes(reportContent.ToString());
                var fileName = "All_Projects_Report.csv";

                return File(reportBytes, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

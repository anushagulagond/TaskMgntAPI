////using Microsoft.AspNetCore.Mvc;
////using System;
////using System.IO;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using TaskMgntAPI.Models;

////namespace TaskMgntAPI.Controllers
////{
////    [Route("api/[controller]")]
////    [ApiController]
////    public class ReportController : ControllerBase
////    {
////        private readonly TaskMgntDbContext _context;

////        public ReportController(TaskMgntDbContext context)
////        {
////            _context = context;
////        }

////        // Fetch project details by ID using LINQ joins
////        [HttpGet("details/{projectId}")]
////        public ActionResult GetAllProjects(int projectId)
////        {
////            try
////            {
////                var projectDetails = (from p in _context.Projects
////                                      join t in _context.TaskTbls on p.ProjectId equals t.ProjectId
////                                      where p.ProjectId == projectId
////                                      select new
////                                      {
////                                          p.ProjectId,
////                                          p.ProjectName,
////                                          p.Description,
////                                          p.StartDate,
////                                          p.EndDate,
////                                          Tasks = _context.TaskTbls
////                                              .Where(task => task.ProjectId == p.ProjectId)
////                                              .Select(task => new
////                                              {
////                                                  task.TaskId,
////                                                  task.TaskName,
////                                                  task.Description,
////                                                  task.TaskStartDate,
////                                                  task.TaskEndDate,
////                                                  task.Priority,
////                                                  task.Status
////                                              }).ToList()
////                                      }).FirstOrDefault();

////                if (projectDetails == null)
////                {
////                    return NotFound("Project not found.");
////                }

////                // Build the response
////                var response = new StringBuilder();
////                response.AppendLine($"Project Name: {projectDetails.ProjectName}");
////                response.AppendLine($"Description: {projectDetails.Description}");
////                response.AppendLine($"Start Date: {projectDetails.StartDate:yyyy-MM-dd}");
////                response.AppendLine($"End Date: {projectDetails.EndDate:yyyy-MM-dd}");
////                response.AppendLine(); 
////                response.AppendLine("Tasks:");
////                response.AppendLine("Task Name,Description,Start Date,End Date,Priority,Status");

////                foreach (var task in projectDetails.Tasks)
////                {
////                    response.AppendLine($"{task.TaskName},{task.Description},{task.TaskStartDate:yyyy-MM-dd},{task.TaskEndDate:yyyy-MM-dd},{task.Priority},{task.Status}");
////                }

////                return Content(response.ToString(), "text/plain");
////            }
////            catch (Exception ex)
////            {
////                return StatusCode(500, $"Internal server error: {ex.Message}");
////            }
////        }

////        // Generate and download project report using LINQ joins
////        [HttpGet("report/{projectId}")]
////        public ActionResult GenerateReport(int projectId)
////        {
////            try
////            {
////                var projectDetails = (from p in _context.Projects
////                                      join t in _context.TaskTbls on p.ProjectId equals t.ProjectId
////                                      where p.ProjectId == projectId
////                                      select new
////                                      {
////                                          p.ProjectId,
////                                          p.ProjectName,
////                                          p.Description,
////                                          p.StartDate,
////                                          p.EndDate,
////                                          Tasks = _context.TaskTbls
////                                              .Where(task => task.ProjectId == p.ProjectId)
////                                              .Select(task => new
////                                              {
////                                                  task.TaskId,
////                                                  task.TaskName,
////                                                  task.Description,
////                                                  task.TaskStartDate,
////                                                  task.TaskEndDate,
////                                                  task.Priority,
////                                                  task.Status
////                                              }).ToList()
////                                      }).FirstOrDefault();

////                if (projectDetails == null)
////                {
////                    return NotFound("Project not found.");
////                }

////                // Build the report content
////                var reportContent = new StringBuilder();
////                reportContent.AppendLine($"Project Name: {projectDetails.ProjectName}");
////                reportContent.AppendLine($"Description: {projectDetails.Description}");
////                reportContent.AppendLine($"Start Date: {projectDetails.StartDate:yyyy-MM-dd}");
////                reportContent.AppendLine($"End Date: {projectDetails.EndDate:yyyy-MM-dd}");
////                reportContent.AppendLine();
////                reportContent.AppendLine("Tasks:");
////                reportContent.AppendLine("Task Name,Description,Start Date,End Date,Priority,Status");

////                foreach (var task in projectDetails.Tasks)
////                {
////                    reportContent.AppendLine($"{task.TaskName},{task.Description},{task.TaskStartDate:yyyy-MM-dd},{task.TaskEndDate:yyyy-MM-dd},{task.Priority},{task.Status}");
////                }

////                // Convert the report content to a byte array for download
////                var reportBytes = Encoding.UTF8.GetBytes(reportContent.ToString());
////                var fileName = $"{projectDetails.ProjectName}_Report.csv";

////                return File(reportBytes, "text/csv", fileName);
////            }
////            catch (Exception ex)
////            {
////                return StatusCode(500, $"Internal server error: {ex.Message}");
////            }
////        }
////    }
////}

//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TaskMgntAPI.Models;

//namespace TaskMgntAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ReportController : ControllerBase
//    {
//        private readonly TaskMgntDbContext _context;

//        public ReportController(TaskMgntDbContext context)
//        {
//            _context = context;
//        }

//        // Generate and download report for all projects
//        [HttpGet("report")]
//        public ActionResult GenerateReport()
//        {
//            try
//            {
//                var projectDetails = (from p in _context.Projects
//                                      join t in _context.TaskTbls on p.ProjectId equals t.ProjectId
//                                      select new
//                                      {
//                                          p.ProjectId,
//                                          p.ProjectName,
//                                          p.Description,
//                                          p.StartDate,
//                                          p.EndDate,
//                                          Tasks = _context.TaskTbls
//                                              .Where(task => task.ProjectId == p.ProjectId)
//                                              .Select(task => new
//                                              {
//                                                  task.TaskId,
//                                                  task.TaskName,
//                                                  task.Description,
//                                                  task.TaskStartDate,
//                                                  task.TaskEndDate,
//                                                  task.Priority,
//                                                  task.Status
//                                              }).ToList()
//                                      }).ToList(); // Fetch all projects and their tasks

//                if (projectDetails == null || !projectDetails.Any())
//                {
//                    return NotFound("No projects found.");
//                }

//                // Build the report content
//                var reportContent = new StringBuilder();
//                reportContent.AppendLine("Project Report");
//                reportContent.AppendLine("====================================");

//                foreach (var project in projectDetails)
//                {
//                    reportContent.AppendLine($"Project Name: {project.ProjectName}");
//                    reportContent.AppendLine($"Description: {project.Description}");
//                    reportContent.AppendLine($"Start Date: {project.StartDate:yyyy-MM-dd}");
//                    reportContent.AppendLine($"End Date: {project.EndDate:yyyy-MM-dd}");
//                    reportContent.AppendLine();
//                    reportContent.AppendLine("Tasks:");
//                    reportContent.AppendLine("Task Name,Description,Start Date,End Date,Priority,Status");

//                    foreach (var task in project.Tasks)
//                    {
//                        reportContent.AppendLine($"{task.TaskName},{task.Description},{task.TaskStartDate:yyyy-MM-dd},{task.TaskEndDate:yyyy-MM-dd},{task.Priority},{task.Status}");
//                    }
//                    reportContent.AppendLine();
//                }

//                // Convert the report content to a byte array for download
//                var reportBytes = Encoding.UTF8.GetBytes(reportContent.ToString());
//                var fileName = "All_Projects_Report.csv";

//                return File(reportBytes, "text/csv", fileName);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text;
using TaskMgntAPI.Models;

namespace TaskMgntAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly TaskMgntDbContext _context;

        public ReportController(TaskMgntDbContext context)
        {
            _context = context;
        }

        // Generate detailed report for a specific project with all tasks
        [HttpGet("detailed-report/{projectId}")]
        public ActionResult GetDetailedReport(int projectId)
        {
            try
            {
                // Fetch project details and tasks using LINQ
                var projectDetails = (from p in _context.Projects
                                      where p.ProjectId == projectId
                                      select new
                                      {
                                          p.ProjectId,
                                          p.ProjectName,
                                          p.Description,
                                          p.StartDate,
                                          p.EndDate,
                                          Tasks = _context.TaskTbls
                                              .Where(t => t.ProjectId == p.ProjectId)
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
                                      }).FirstOrDefault();

                if (projectDetails == null)
                {
                    return NotFound("Project not found.");
                }

                // Build the response content
                var reportContent = new StringBuilder();
                reportContent.AppendLine("Project Report");
                reportContent.AppendLine("====================================");
                reportContent.AppendLine($"Project Name: {projectDetails.ProjectName}");
                reportContent.AppendLine($"Description: {projectDetails.Description}");
                reportContent.AppendLine($"Start Date: {projectDetails.StartDate:yyyy-MM-dd}");
                reportContent.AppendLine($"End Date: {projectDetails.EndDate:yyyy-MM-dd}");
                reportContent.AppendLine();
                reportContent.AppendLine("Tasks:");
                reportContent.AppendLine("Task Name,Description,Start Date,End Date,Priority,Status");

                foreach (var task in projectDetails.Tasks)
                {
                    reportContent.AppendLine($"{task.TaskName},{task.Description},{task.TaskStartDate:yyyy-MM-dd},{task.TaskEndDate:yyyy-MM-dd},{task.Priority},{task.Status}");
                }

                return Content(reportContent.ToString(), "text/plain");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Generate a consolidated report for all projects
        [HttpGet("consolidated-report")]
        public ActionResult GenerateConsolidatedReport()
        {
            try
            {
                var projectDetails = (from p in _context.Projects
                                      select new
                                      {
                                          p.ProjectId,
                                          p.ProjectName,
                                          p.Description,
                                          p.StartDate,
                                          p.EndDate,
                                          Tasks = _context.TaskTbls
                                              .Where(t => t.ProjectId == p.ProjectId)
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
                                      }).ToList();

                if (!projectDetails.Any())
                {
                    return NotFound("No projects found.");
                }

                var reportContent = new StringBuilder();
                reportContent.AppendLine("All Projects Report");
                reportContent.AppendLine("====================================");

                foreach (var project in projectDetails)
                {
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

                    reportContent.AppendLine(); // Separate projects
                }

                var reportBytes = Encoding.UTF8.GetBytes(reportContent.ToString());
                var fileName = "Consolidated_Projects_Report.csv";

                return File(reportBytes, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}


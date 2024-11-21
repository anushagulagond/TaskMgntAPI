using System.ComponentModel.DataAnnotations;

namespace TaskMgntAPI.Models
{
    public class TaskReport
    {
        [Key]
        // public int TaskReportId {  get; set; }
        // public int TaskId { get; set; }
        // public string TaskName { get; set; }
        // public decimal EstimatedCost { get; set; }
        // public DateTime TaskStartDate { get; set; }
        // public DateTime TaskEndDate { get; set; }
        //public string TaskStatus {  get; set; }

            public int TaskId { get; set; }
            public string TaskName { get; set; }
            public string Status { get; set; } // Not Started, In Progress, Completed
            public string Priority { get; set; } // Low, Medium, High
            public int ProjectId { get; set; }
            public Project Project { get; set; }

            public decimal EstimatedCost { get; set; }
            public DateTime TaskStartDate { get; set; }
            public DateTime TaskEndDate { get; set; }
    }

}

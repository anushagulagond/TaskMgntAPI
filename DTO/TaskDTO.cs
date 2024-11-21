namespace TaskMgntAPI.DTO
{
    public class TaskDTO
    {
        public int TaskId { get; set; }

        public string TaskName { get; set; } = null!;

        public int ProjectId { get; set; }

        public string AssignedToUserId { get; set; } = null!;

        public DateTime TaskStartDate { get; set; }

        public DateTime TaskEndDate { get; set; }

        public string Priority { get; set; } = null!;

        public string Status { get; set; } = null!;
        public string? Description { get; set; }

    }
}

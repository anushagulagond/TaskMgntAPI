using System;
using System.Collections.Generic;

namespace TaskMgntAPI.Models;

public partial class Timesheet
{
    public int TimesheetId { get; set; }

    public int TaskId { get; set; }

    public DateTime Date { get; set; }

    public decimal HoursWorked { get; set; }

    public virtual Task Task { get; set; } = null!;
}

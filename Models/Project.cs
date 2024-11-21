﻿using System;
using System.Collections.Generic;

namespace TaskMgntAPI.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public string ProjectName { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<TaskTbl> TaskTbls { get; set; } = new List<TaskTbl>();
}

using System;
using System.Collections.Generic;

namespace TaskMgntAPI.Models;

public partial class AspNetUser
{
    public string Id { get; set; } = null!;

    public string? Email { get; set; }

    public bool? EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool? PhoneNumberConfirmed { get; set; }

    public bool? TwoFactorEnabled { get; set; }

    public DateTime? LockoutEndDateUtc { get; set; }

    public bool? LockoutEnabled { get; set; }

    public int? AccessFailedCount { get; set; }

    public string UserName { get; set; } = null!;

    public string? UserRole { get; set; }

    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    public virtual ICollection<TaskTbl> TaskTbls { get; set; } = new List<TaskTbl>();

    public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();
}

using System;
using System.Collections.Generic;

namespace NCKH_HRM.Models;

public partial class Staff
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public DateTime? BirthDate { get; set; }

    public string? NumberPhone { get; set; }

    public bool? Gender { get; set; }

    public string? Email { get; set; }

    public string? Degree { get; set; }

    public int? Major { get; set; }

    public int? Yearofwork { get; set; }

    public int? Position { get; set; }

    public string? CreateBy { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDelete { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<CoursePoint> CoursePoints { get; set; } = new List<CoursePoint>();

    public virtual Major? MajorNavigation { get; set; }

    public virtual Position? PositionNavigation { get; set; }

    public virtual ICollection<StaffSubject> StaffSubjects { get; set; } = new List<StaffSubject>();

    public virtual ICollection<TeachingAssignment> TeachingAssignments { get; set; } = new List<TeachingAssignment>();

    public virtual ICollection<UserStaff> UserStaffs { get; set; } = new List<UserStaff>();
}

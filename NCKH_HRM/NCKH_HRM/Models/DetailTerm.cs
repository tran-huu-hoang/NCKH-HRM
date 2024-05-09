using System;
using System.Collections.Generic;

namespace NCKH_HRM.Models;

public partial class DetailTerm
{
    public long Id { get; set; }

    public long? Term { get; set; }

    public int? Semester { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Room { get; set; }

    public int? MaxNumber { get; set; }

    public string? CreateBy { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDelete { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<CoursePoint> CoursePoints { get; set; } = new List<CoursePoint>();

    public virtual ICollection<DateLearn> DateLearns { get; set; } = new List<DateLearn>();

    public virtual ICollection<DetailAtteandance> DetailAtteandances { get; set; } = new List<DetailAtteandance>();

    public virtual ICollection<PointProcess> PointProcesses { get; set; } = new List<PointProcess>();

    public virtual ICollection<RegistStudent> RegistStudents { get; set; } = new List<RegistStudent>();

    public virtual Semester? SemesterNavigation { get; set; }

    public virtual ICollection<TeachingAssignment> TeachingAssignments { get; set; } = new List<TeachingAssignment>();

    public virtual Term? TermNavigation { get; set; }

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}

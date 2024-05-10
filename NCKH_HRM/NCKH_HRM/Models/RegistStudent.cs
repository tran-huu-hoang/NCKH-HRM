using System;
using System.Collections.Generic;

namespace NCKH_HRM.Models;

public partial class RegistStudent
{
    public long Id { get; set; }

    public long? Student { get; set; }

    public long? DetailTerm { get; set; }

    public int? Relearn { get; set; }

    public bool? Status { get; set; }

    public string? CreateBy { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDelete { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<CoursePoint> CoursePoints { get; set; } = new List<CoursePoint>();

    public virtual ICollection<DateLearn> DateLearns { get; set; } = new List<DateLearn>();

    public virtual DetailTerm? DetailTermNavigation { get; set; }

    public virtual ICollection<PointProcess> PointProcesses { get; set; } = new List<PointProcess>();

    public virtual Student? StudentNavigation { get; set; }

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}

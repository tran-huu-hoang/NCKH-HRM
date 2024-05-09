using System;
using System.Collections.Generic;

namespace NCKH_HRM.Models;

public partial class PointSys4
{
    public int Id { get; set; }

    public double? Range1 { get; set; }

    public double? Range2 { get; set; }

    public double? Point { get; set; }

    public string? CreateBy { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDelete { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<CoursePoint> CoursePoints { get; set; } = new List<CoursePoint>();
}

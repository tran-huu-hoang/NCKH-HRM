using System;
using System.Collections.Generic;

namespace NCKH_HRM.Models;

public partial class Test
{
    public long Id { get; set; }

    public long? IdAttendance { get; set; }

    public long? DetailTerm { get; set; }

    public long? DateLearn { get; set; }

    public bool? Status { get; set; }

    public virtual DateLearn? DateLearnNavigation { get; set; }

    public virtual DetailTerm? DetailTermNavigation { get; set; }

    public virtual Attendance? IdAttendanceNavigation { get; set; }

    public virtual ICollection<PointProcess> PointProcesses { get; set; } = new List<PointProcess>();
}

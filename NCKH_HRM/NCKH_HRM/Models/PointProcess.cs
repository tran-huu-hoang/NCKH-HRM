using System;
using System.Collections.Generic;

namespace NCKH_HRM.Models;

public partial class PointProcess
{
    public long Id { get; set; }

    public long? Student { get; set; }

    public long? DetailTerm { get; set; }

    public long? RegistStudent { get; set; }

    public int? ScoreBoard { get; set; }

    public long? Attendance { get; set; }

    public long? Test { get; set; }

    public double? PointProcess1 { get; set; }

    public bool? Status { get; set; }

    public string? CreateBy { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDelete { get; set; }

    public bool? IsActive { get; set; }

    public virtual Attendance? AttendanceNavigation { get; set; }

    public virtual DetailTerm? DetailTermNavigation { get; set; }

    public virtual RegistStudent? RegistStudentNavigation { get; set; }

    public virtual ScoreBoard? ScoreBoardNavigation { get; set; }

    public virtual Student? StudentNavigation { get; set; }

    public virtual Test? TestNavigation { get; set; }
}

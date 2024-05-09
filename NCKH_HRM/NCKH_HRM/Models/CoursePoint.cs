﻿using System;
using System.Collections.Generic;

namespace NCKH_HRM.Models;

public partial class CoursePoint
{
    public long Id { get; set; }

    public long? Student { get; set; }

    public long? DetailTerm { get; set; }

    public long? RegistStudent { get; set; }

    public int? ScoreBoard { get; set; }

    public double? TestScore { get; set; }

    public double? OverallScore { get; set; }

    public int? Overall4 { get; set; }

    public int? NumberTest { get; set; }

    public bool? Status { get; set; }

    public long? IdStaff { get; set; }

    public string? CreateBy { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDelete { get; set; }

    public bool? IsActive { get; set; }

    public virtual DetailTerm? DetailTermNavigation { get; set; }

    public virtual Staff? IdStaffNavigation { get; set; }

    public virtual PointSys4? Overall4Navigation { get; set; }

    public virtual RegistStudent? RegistStudentNavigation { get; set; }

    public virtual ScoreBoard? ScoreBoardNavigation { get; set; }

    public virtual Student? StudentNavigation { get; set; }
}

using System;
using System.Collections.Generic;

namespace NCKH_HRM.Models;

public partial class DetailAtteandance
{
    public long Id { get; set; }

    public long? Student { get; set; }

    public long? DetailTerm { get; set; }

    public long? RegistStudent { get; set; }

    public double? Test1 { get; set; }

    public double? Test2 { get; set; }

    public double? Test3 { get; set; }

    public double? Test4 { get; set; }

    public double? Test5 { get; set; }

    public double? Testavg { get; set; }

    public int? CountAttend { get; set; }

    public int? CountLearn { get; set; }

    public string? CreateBy { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDelete { get; set; }

    public bool? IsActive { get; set; }

    public virtual DetailTerm? DetailTermNavigation { get; set; }

    public virtual RegistStudent? RegistStudentNavigation { get; set; }

    public virtual Student? StudentNavigation { get; set; }
}

using System;
using System.Collections.Generic;

namespace NCKH_HRM.Models;

public partial class Timeline
{
    public long Id { get; set; }

    public DateTime? DateLearn { get; set; }

    public long? Year { get; set; }

    public bool? Status { get; set; }

    public string? CreateBy { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? Isdelete { get; set; }

    public bool? Isactive { get; set; }

    public virtual ICollection<DateLearn> DateLearns { get; set; } = new List<DateLearn>();

    public virtual Year? YearNavigation { get; set; }
}

using System;
using System.Collections.Generic;

namespace NCKH_HRM.Models;

public partial class Year
{
    public long Id { get; set; }

    public int? Name { get; set; }

    public virtual ICollection<Timeline> Timelines { get; set; } = new List<Timeline>();
}

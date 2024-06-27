﻿using System;
using System.Collections.Generic;

namespace NCKH_HRM.Models;

public partial class Room
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<DateLearn> DateLearns { get; set; } = new List<DateLearn>();
}

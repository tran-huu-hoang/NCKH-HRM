﻿using System;
using System.Collections.Generic;

namespace NCKH_HRM.Models;

public partial class Position
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? CreateBy { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDelete { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}

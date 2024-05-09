using System;
using System.Collections.Generic;

namespace NCKH_HRM.Models;

public partial class UserStudent
{
    public long Id { get; set; }

    public long? Student { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public long? CreateBy { get; set; }

    public long? UpdateBy { get; set; }

    public string? CreateDate { get; set; }

    public string? UpdateDate { get; set; }

    public DateTime? IsDelete { get; set; }

    public DateTime? IsActive { get; set; }

    public virtual Student? StudentNavigation { get; set; }
}

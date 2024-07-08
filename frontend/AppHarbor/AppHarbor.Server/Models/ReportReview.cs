using System;
using System.Collections.Generic;

namespace AppHarbor.Server.Models;

public partial class ReportReview
{
    public decimal ReportId { get; set; }

    public decimal AdminId { get; set; }

    public DateTime? ReceiveTime { get; set; }

    public DateTime? ReviewTime { get; set; }

    public string Result { get; set; } = null!;

    public virtual Admin Admin { get; set; } = null!;

    public virtual Report Report { get; set; } = null!;
}

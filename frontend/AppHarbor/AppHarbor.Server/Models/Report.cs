using System;
using System.Collections.Generic;

namespace AppHarbor.Server.Models;

public partial class Report
{
    public decimal Id { get; set; }

    public string? Content { get; set; }

    public DateTime Time { get; set; }

    public decimal UserId { get; set; }

    public decimal ApplicationId { get; set; }

    public virtual Application Application { get; set; } = null!;

    public virtual ReportReview? ReportReview { get; set; }

    public virtual User User { get; set; } = null!;
}

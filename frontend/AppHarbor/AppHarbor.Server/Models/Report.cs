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

    public string State { get; set; } = null!;

    public virtual Application Application { get; set; } = null!;

    public virtual ReportReview? ReportReview { get; set; }

    public virtual User User { get; set; } = null!;
}

public class PublishReportModel
{
    public string Token { get; set; } = null!;

    public string Content { get; set; } = null!;

    public decimal ApplicationId { get; set; }

    public DateTime ReportTime { get; set; }
}

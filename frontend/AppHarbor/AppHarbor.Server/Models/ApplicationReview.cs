using System;
using System.Collections.Generic;

namespace AppHarbor.Server.Models;

public partial class ApplicationReview
{
    public decimal ApplicationId { get; set; }

    public decimal AdminId { get; set; }

    public DateTime? ReviewTime { get; set; }

    public string Result { get; set; } = null!;

    public virtual Admin Admin { get; set; } = null!;

    public virtual Application Application { get; set; } = null!;
}

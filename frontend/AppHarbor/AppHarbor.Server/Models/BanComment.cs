using System;
using System.Collections.Generic;

namespace AppHarbor.Server.Models;

public partial class BanComment
{
    public decimal AdminId { get; set; }

    public decimal CommentId { get; set; }

    public DateTime? Time { get; set; }

    public string? Reason { get; set; }

    public virtual Admin Admin { get; set; } = null!;

    public virtual Comment Comment { get; set; } = null!;
}

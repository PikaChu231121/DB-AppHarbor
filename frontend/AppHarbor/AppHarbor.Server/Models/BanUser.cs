using System;
using System.Collections.Generic;

namespace AppHarbor.Server.Models;

public partial class BanUser
{
    public decimal AdminId { get; set; }

    public decimal UserId { get; set; }

    public DateTime? Time { get; set; }

    public string? Reason { get; set; }

    public virtual Admin Admin { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

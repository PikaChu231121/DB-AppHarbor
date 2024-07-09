using System;
using System.Collections.Generic;

namespace AppHarbor.Server.Models;

public partial class BanMerchant
{
    public decimal AdminId { get; set; }

    public decimal MerchantId { get; set; }

    public string? Reason { get; set; }

    public DateTime? Time { get; set; }

    public virtual Admin Admin { get; set; } = null!;

    public virtual Merchant Merchant { get; set; } = null!;
}

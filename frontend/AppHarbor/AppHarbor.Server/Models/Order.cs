using System;
using System.Collections.Generic;

namespace AppHarbor.Server.Models;

public partial class Order
{
    public decimal Id { get; set; }

    public DateTime? Time { get; set; }

    public decimal Amount { get; set; }

    public decimal ApplicationId { get; set; }

    public decimal BuyerId { get; set; }

    public decimal ReceiverId { get; set; }

    public virtual Application Application { get; set; } = null!;

    public virtual User Buyer { get; set; } = null!;

    public virtual User Receiver { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace AppHarbor.Server.Models;

public partial class Merchant
{
    public decimal Id { get; set; }

    public string Password { get; set; } = null!;

    public string Nickname { get; set; } = null!;

    public string? Avatar { get; set; }

    public DateTime RegisterTime { get; set; }

    public decimal? Credit { get; set; }

    public string State { get; set; } = null!;

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ICollection<BanMerchant> BanMerchants { get; set; } = new List<BanMerchant>();
}

public class MerchanrWithdrawModel 
{
    public string Token { get; set; }

    public decimal Amount { get; set; }
}
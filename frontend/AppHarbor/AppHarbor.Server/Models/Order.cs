using System;
using System.Collections.Generic;

namespace AppHarbor.Server.Models;

public partial class Order
{
    public decimal Id { get; set; }

    public DateTime? Time { get; set; }

    public decimal Amount { get; set; }

    public decimal? ApplicationId { get; set; } // 若本次交易类型为充值，则ApplicationId为NULL

    public decimal BuyerId { get; set; }

    public decimal? ReceiverId { get; set; } // 若本次交易类型为充值，则ReceiverId为NULL

    public string Type { get; set; } = "purchase"; // 交易类型未指明，则默认为购买

    public virtual Application Application { get; set; } = null!;

    public virtual User Buyer { get; set; } = null!;

    public virtual User Receiver { get; set; } = null!;
}

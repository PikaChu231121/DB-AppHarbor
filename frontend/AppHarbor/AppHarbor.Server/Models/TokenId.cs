using System;
using System.Collections.Generic;

namespace AppHarbor.Server.Models;

public partial class TokenId
{
    public string Token { get; set; } = null!;

    public DateTime ExpireDate { get; set; }

    public decimal Id { get; set; }

    public virtual User IdNavigation { get; set; } = null!;
}

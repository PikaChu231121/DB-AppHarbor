using System;
using System.Collections.Generic;

namespace AppHarbor.Server.Models;

public partial class Relationship
{
    public decimal User1Id { get; set; }

    public decimal User2Id { get; set; }

    public DateTime? CreateTime { get; set; }

    public string Relationship1 { get; set; } = null!;

    public virtual User User1 { get; set; } = null!;

    public virtual User User2 { get; set; } = null!;
}

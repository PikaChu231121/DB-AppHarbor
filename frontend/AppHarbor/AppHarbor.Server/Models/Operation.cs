using System;
using System.Collections.Generic;

namespace AppHarbor.Server.Models;

public partial class Operation
{
    public decimal ApplicationId { get; set; }

    public decimal AdminId { get; set; }

    public string Version { get; set; } = null!;

    public string OperationType { get; set; } = null!;

    public DateTime OperationTime { get; set; }

    public virtual Admin Admin { get; set; } = null!;

    public virtual Application Application { get; set; } = null!;
}

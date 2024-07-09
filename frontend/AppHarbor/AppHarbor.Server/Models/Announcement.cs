﻿using System;
using System.Collections.Generic;

namespace AppHarbor.Server.Models;

public partial class Announcement
{
    public decimal Id { get; set; }

    public string? Title { get; set; }

    public string Content { get; set; } = null!;

    public decimal AdminId { get; set; }

    public DateTime PublishTime { get; set; }

    public virtual Admin Admin { get; set; } = null!;
}

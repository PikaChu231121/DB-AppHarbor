using System;
using System.Collections.Generic;

namespace AppHarbor.Server.Models;

public class UserFavouriteModel
{
    public decimal Id { get; set; }
}
public partial class Favourite
{
    public decimal Id { get; set; }

    public string? Name { get; set; }

    public DateTime? CreateTime { get; set; }

    public string? Visibility { get; set; }

    public decimal UserId { get; set; }

    public decimal ApplicationId { get; set; }

    public virtual Application Application { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

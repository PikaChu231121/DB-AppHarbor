using System;
using System.Collections.Generic;

namespace AppHarbor.Server.Models;

public class GetFavouritesModel
{
    public decimal UserId { get; set; }
}
public class DeleteFavouriteRequest
{
    public string Token { get; set; }
    public decimal Id { get; set; }
}

public class BulkDeleteRequest
{
    public string Token { get; set; }
    public List<decimal> Ids { get; set; }
}

public partial class Favourite
{
    public decimal Id { get; set; }

    // public string? Name { get; set; }

    public DateTime? CreateTime { get; set; }

    public string? Visibility { get; set; }

    public decimal UserId { get; set; }

    public decimal ApplicationId { get; set; }

    public virtual Application Application { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

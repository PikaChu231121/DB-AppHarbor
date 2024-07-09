using System;
using System.Collections.Generic;

namespace AppHarbor.Server.Models;

public partial class Application
{
    public decimal Id { get; set; }

    public string Version { get; set; } = null!;

    public decimal MerchantId { get; set; }

    public string Name { get; set; } = null!;

    public string? Category { get; set; }

    public string Description { get; set; } = null!;

    public string ReleaseState { get; set; } = null!;

    public string? Image { get; set; }

    public decimal? DownloadCount { get; set; }

    public virtual ICollection<ApplicationReview> ApplicationReviews { get; set; } = new List<ApplicationReview>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();

    public virtual Merchant Merchant { get; set; } = null!;

    public virtual ICollection<Operation> Operations { get; set; } = new List<Operation>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}

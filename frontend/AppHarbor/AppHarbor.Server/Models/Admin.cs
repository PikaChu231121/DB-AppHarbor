using System;
using System.Collections.Generic;

namespace AppHarbor.Server.Models;

public partial class Admin
{
    public decimal Id { get; set; }

    public string Password { get; set; } = null!;

    public string Nickname { get; set; } = null!;

    public string? Avatar { get; set; }

    public DateTime RegisterTime { get; set; }

    public decimal? Credit { get; set; }

    public string State { get; set; } = null!;

    public virtual ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();

    public virtual ICollection<ApplicationReview> ApplicationReviews { get; set; } = new List<ApplicationReview>();

    public virtual ICollection<BanMerchant> BanMerchants { get; set; } = new List<BanMerchant>();

    public virtual ICollection<BanUser> BanUsers { get; set; } = new List<BanUser>();

    public virtual ICollection<Operation> Operations { get; set; } = new List<Operation>();

    public virtual ICollection<ReportReview> ReportReviews { get; set; } = new List<ReportReview>();
}

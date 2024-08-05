using System;
using System.Collections.Generic;

namespace AppHarbor.Server.Models;



public class GetAppCommentModel
{
    public decimal ApplicationId { get; set; }
}

public class PostAppCommentModel
{
    public required string Token { get; set; }
    public required string Content { get; set; }
    public decimal Rating { get; set; }
    public decimal ApplicationId { get; set; }
}


public partial class Comment
{
    public decimal Id { get; set; }

    public string Content { get; set; } = null!;

    public decimal? Score { get; set; }

    public decimal ApplicationId { get; set; }

    public decimal UserId { get; set; }

    public DateTime PublishTime { get; set; }

    public virtual Application Application { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

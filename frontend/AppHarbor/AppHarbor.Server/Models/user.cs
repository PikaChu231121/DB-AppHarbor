﻿using System;
using System.Collections.Generic;

namespace AppHarbor.Server.Models;

public partial class User
{
    public decimal Id { get; set; }

<<<<<<< Updated upstream
    public class user
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
=======
    public string Password { get; set; } = null!;
>>>>>>> Stashed changes

    public string Nickname { get; set; } = null!;

    public string? Avatar { get; set; }

    public DateTime RegisterTime { get; set; }

    public decimal? Credit { get; set; }

    public string State { get; set; } = null!;

    public virtual ICollection<BanUser> BanUsers { get; set; } = new List<BanUser>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();

    public virtual ICollection<Order> OrderBuyers { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderReceivers { get; set; } = new List<Order>();

    public virtual ICollection<Relationship> RelationshipUser1s { get; set; } = new List<Relationship>();

    public virtual ICollection<Relationship> RelationshipUser2s { get; set; } = new List<Relationship>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}

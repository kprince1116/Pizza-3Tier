using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Rating
{
    public int Ratingid { get; set; }

    public decimal? Foodrating { get; set; }

    public decimal? Ambiencerating { get; set; }

    public decimal? Servicerating { get; set; }

    public string? Comments { get; set; }

    public decimal? Avgrating { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

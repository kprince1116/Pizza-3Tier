using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Rating
{
    public int Ratingid { get; set; }

    public int? Foodrating { get; set; }

    public int? Ambiencerating { get; set; }

    public int? Servicerating { get; set; }

    public string? Comments { get; set; }

    public int Avgrating { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Orderstatus
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

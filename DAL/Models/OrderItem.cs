using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class OrderItem
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int? ItemId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public virtual MenuItem? Item { get; set; }

    public virtual Order? Order { get; set; }

    public virtual ICollection<OrderItemModifier> OrderItemModifiers { get; set; } = new List<OrderItemModifier>();
}

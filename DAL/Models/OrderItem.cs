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

    public string? Status { get; set; }

    public int? ReadyItem { get; set; }

    public decimal? ItemTaxPercenetage { get; set; }

    public string? OrderItemInstruction { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual MenuItem? Item { get; set; }

    public virtual Order? Order { get; set; }

    public virtual ICollection<OrderItemModifier> OrderItemModifiers { get; set; } = new List<OrderItemModifier>();
}

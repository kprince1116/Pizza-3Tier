using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class MenuItem
{
    public int Itemid { get; set; }

    public int? Categoryid { get; set; }

    public string Itemname { get; set; } = null!;

    public string? Itemtype { get; set; }

    public int? Quantity { get; set; }

    public int? Unitid { get; set; }

    public decimal? Rate { get; set; }

    public bool? IsAvailable { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public decimal? TaxPercentage { get; set; }

    public bool? IsFavourite { get; set; }

    public string? ShortCode { get; set; }

    public bool? IsDefaultTax { get; set; }

    public bool? IsDeleted { get; set; }

    public long? CreatedBy { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual MenuCategory? Category { get; set; }

    public virtual ICollection<Itemmodifiergroup> Itemmodifiergroups { get; set; } = new List<Itemmodifiergroup>();

    public virtual Unit? Unit { get; set; }
}

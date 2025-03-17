using DAL.Models;

namespace Pizzashop.DAL.ViewModels;

public class AddItemviewmodel
{

    public int Itemid { get; set; }

    public int? Categoryid { get; set; }

    public string Itemname { get; set; } = null!;

    public string? Itemtype { get; set; }

    public int? Quantity { get; set; }

    public decimal? Rate { get; set; }

    public bool IsAvailable { get; set; }

    public string? Image { get; set; }
    public string? Description { get; set; }

    public decimal? TaxPercentage { get; set; }

    public bool? IsFavourite { get; set; }

    public string? ShortCode { get; set; }

    public bool IsDefaultTax { get; set; }

    public virtual MenuCategory? Category { get; set; }

    public virtual Unit? Unit { get; set; }

}

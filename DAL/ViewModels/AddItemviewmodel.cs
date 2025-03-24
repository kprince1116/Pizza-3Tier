using System.ComponentModel.DataAnnotations;
using DAL.Models;
using Microsoft.AspNetCore.Http;

namespace Pizzashop.DAL.ViewModels;

public class AddItemviewmodel
{

    public IEnumerable<ModifierGroupViewModel> ModifierGroupList {get; set;}

    public List<ItemModifierGroupviewmodel> ItemModifierList { get; set; } = new List<ItemModifierGroupviewmodel>();


    public int Itemid { get; set; }

    public int? Categoryid { get; set; }

    [Required(ErrorMessage = "ItemName is required")]
    public string? Itemname { get; set; } = null!;

    [Required(ErrorMessage = "ItemType is required")]
    public string? Itemtype { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    public int? Quantity { get; set; }

    [Required(ErrorMessage = "Rate is required")]
    public decimal? Rate { get; set; }

    public bool IsAvailable { get; set; }
    public IFormFile ItemImage { get; set; }
    public string? Image { get; set; }

    public string? Description { get; set; }

    [Required(ErrorMessage = "TaxPercentage is required")]
    public decimal? TaxPercentage { get; set; }

    public bool? IsFavourite { get; set; }

    [Required(ErrorMessage = "ShortCode is required")]
    public string? ShortCode { get; set; }

    public bool IsDefaultTax { get; set; }

    public virtual MenuCategory? Category { get; set; }

    public virtual Unit? Unit { get; set; }

}

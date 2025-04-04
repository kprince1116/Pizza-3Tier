using DAL.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace Pizzashop.DAL.ViewModels;

public class AddModifierViewModel
{
    public List<ItemModifierGroupviewmodel>? ItemModifierList { get; set; }

    public IEnumerable<ModifierGroup> modifiers { get; set; }

     public IEnumerable<Unit> units {get; set;} 

    [Required(ErrorMessage = "ModifierName is required")]
    public string ModifierName { get; set; } 

    public int? ModifierGroupId { get; set; }
    public int? ModifierId { get; set; }

   [Required(ErrorMessage = "Quantity is required")]
    public int? Quantity { get; set; }

    [Required(ErrorMessage = "Rate is required")]
    public decimal? Rate { get; set; }

    [Required(ErrorMessage = "Unit is required")]
    public int? UnitId { get; set; }

    [Required(ErrorMessage = "Unit is required")]
    public string unit {get ; set;}
    public string? Description { get; set; }

    public decimal? TaxPercentage { get; set; }

    public virtual ModifierGroup? Modifier { get; set; }

    public virtual Unit? Unit { get; set; }

}

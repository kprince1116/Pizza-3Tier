using DAL.Models;

namespace Pizzashop.DAL.ViewModels;

public class AddModifierViewModel
{
      public IEnumerable<ModifierGroup> modifiers { get; set; }

     public IEnumerable<Unit> units {get; set;} 
    public string ModifierName { get; set; } 

    public int? ModifierGroupId { get; set; }
    public int? ModifierId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Rate { get; set; }

    public int? UnitId { get; set; }

    public string? Description { get; set; }

    public decimal? TaxPercentage { get; set; }

    public virtual ModifierGroup? Modifier { get; set; }

    public virtual Unit? Unit { get; set; }

}

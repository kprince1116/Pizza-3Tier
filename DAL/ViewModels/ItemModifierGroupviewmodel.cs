namespace Pizzashop.DAL.ViewModels;

public class ItemModifierGroupviewmodel
{
    public int ItemId { get; set; } 
    public int ModifierGroupId { get; set;}
    public string? Name { get; set; }
    public int? MinAllowed { get; set; }
    public int? MaxAllowed { get; set; }

    public List<ModifierItemViewModel> ModifierItemList  { get; set; } = new List<ModifierItemViewModel>();
}

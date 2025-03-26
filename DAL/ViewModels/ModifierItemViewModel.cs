namespace Pizzashop.DAL.ViewModels;

public class ModifierItemViewModel
{
    public int ModifierGroupId { get; set; }
    public int ModifierItemId { get; set; }
    public string Name { get; set; }
    public decimal? Rate { get; set; }
    public int? Unit { get; set; }
    public string Unitname { get; set; }
    
    public int? Quantity { get; set; }
}

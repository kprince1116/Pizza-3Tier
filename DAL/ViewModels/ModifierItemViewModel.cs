namespace Pizzashop.DAL.ViewModels;

public class ModifierItemViewModel
{
    public long ModifierItemId { get; set; }
    public string Name { get; set; }
    public decimal? Rate { get; set; }
    public int? Unit { get; set; }
    public int? Quantity { get; set; }
}

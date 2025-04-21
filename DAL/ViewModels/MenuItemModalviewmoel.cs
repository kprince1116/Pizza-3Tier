namespace Pizzashop.DAL.ViewModels;

public class MenuItemModalviewmoel
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } 
    public List<MenuModifierGroupViewModel> ModifierGroupList { get; set; } = new List<MenuModifierGroupViewModel>();
}

public class MenuModifierGroupViewModel
{
    public int ModifierGroupId { get; set; }
    public string ModifierGroupName { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }
    public List<MenuModifierViewModel> ModifierList { get; set; } = new List<MenuModifierViewModel>();
}

public class MenuModifierViewModel
{
    public int ModifierId { get; set; }
    public string ModifierName { get; set; }
    public decimal Price { get; set; }
}
using System.ComponentModel.Design.Serialization;

namespace Pizzashop.DAL.ViewModels;

public class OrderCardviewmodel
{
    public int OrderID { get; set; }  

    public int OrderNo { get; set; }  

    public string OrderStatus { get; set; }

    public List<OrderItemsviewmodel> ItemList { get; set; } = new List<OrderItemsviewmodel>();



public class OrderItemsviewmodel
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }

    public int OrderItemID {get; set;}

    public bool IsSelected { get; set; } = false;

    public int TotalQuantity { get; set; }

    public int PendigQuantity { get; set; }

    public int ReadyQuantity { get; set; }

    public List<OrderItemModifierviewmodel> ModifierList { get; set; } = new List<OrderItemModifierviewmodel>();
}

public class OrderItemModifierviewmodel
{
    public int ModifierId { get; set; }
    public string ModifierName { get; set; }

}
}
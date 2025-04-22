using DAL.Models;

namespace Pizzashop.DAL.ViewModels;

public class OrderAppMenuviewmodel
{
    public int CustomerId { get; set; }
    public int OrderId { get; set; }
    public List<MenuCategoryviewmodel> categories { get; set; } = new List<MenuCategoryviewmodel>();
    public List<Itemsviewmodel> items {get; set;} = new List<Itemsviewmodel>();
    public List<tableviewmodel> tables = new List<tableviewmodel>();
    public List<OrderItemviewmodel> orderitems = new List<OrderItemviewmodel>();
}

public class MenuCategoryviewmodel
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } 
   
}

public class Itemsviewmodel
{
     public int CategoryId { get; set; }

     public int ItemId {get; set;}

     public string ItemName {get; set;}

     public string ItemType {get; set;}

     public decimal price {get; set;}

     public bool isFavourite {get; set;}
}

public class tableviewmodel
{
    public int sectionId { get; set; }

    public string sectionName { get; set; }
    public int TableId { get; set; }
    public string TableName { get; set; } 
 
}

public class OrderItemviewmodel
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public decimal price { get; set; }
    public int Quantity { get; set; } 
    public decimal TotalAmount { get; set; }
    public List<ModiferListModel> modifiers { get; set; } = new List<ModiferListModel>();
}

public class ModiferListModel
{
    public int ModifierId { get; set; }
    public string ModifierName { get; set; }
    public decimal price { get; set; }
    public int Quantity { get; set; }
}
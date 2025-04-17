using DAL.Models;

namespace Pizzashop.DAL.ViewModels;

public class OrderAppMenuviewmodel
{
    public List<MenuCategoryviewmodel> categories { get; set; } = new List<MenuCategoryviewmodel>();

    public List<Itemsviewmodel> items {get; set;} = new List<Itemsviewmodel>();
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
}
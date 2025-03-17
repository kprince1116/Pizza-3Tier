using Pizzashop.DAL.ViewModels;

namespace Pizzashop.DAL.ViewModels;

public class ItemListviewmodel
{
    public int? categoryId {get; set;}

    public IEnumerable<MenuItemsviewmodel> Items { get; set; }
    public paginationviewmodel? Page{get; set;}

}

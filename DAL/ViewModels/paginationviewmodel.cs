using DAL.Models;

namespace Pizzashop.DAL.ViewModels;

public class paginationviewmodel
{

     public IEnumerable<MenuItemsviewmodel> Items { get; set; }
     public int CurrentPage {get; set;}
     public int TotalPages {get; set;}
     public int PageSize {get; set;}
     public int TotalRecords {get; set;}
     public string searchKey {get; set;}

     public int Modifierid { get; set; }
     public int Categoryid { get; set; }


}

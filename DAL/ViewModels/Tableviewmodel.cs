using DAL.ViewModels;

namespace Pizzashop.DAL.ViewModels;

public class Tableviewmodel
{
     public IEnumerable<TableItemviewmodel> TableItems { get; set; }
     public int CurrentPage {get; set;}
     public int TotalPages {get; set;}
     public int PageSize {get; set;}
     public int TotalRecords {get; set;}
     public string searchKey {get; set;}

     public int Sectionid { get; set; }
}

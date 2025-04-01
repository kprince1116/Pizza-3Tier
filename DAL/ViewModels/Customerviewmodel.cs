namespace Pizzashop.DAL.ViewModels;

public class Customerviewmodel
{
     public IEnumerable<CustomerDetailsviewmodel> Customers {get; set;}

     public int CurrentPage {get; set;}
     public int TotalPages {get; set;}
     public int PageSize {get; set;}
     public int TotalRecords {get; set;}
     public string searchKey {get; set;}
     public string sortDirection {get; set;}
     public string SortBy {get; set;}
}

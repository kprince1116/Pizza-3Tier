using DAL.ViewModels;

namespace Pizzashop.DAL.ViewModels;

public class Taxviewmodel
{
    public IEnumerable<TaxListviewmodel> TaxList { get; set; } 
    public pageviewmodel? Page{ get; set; }
     public int CurrentPage {get; set;}
     public int TotalPages {get; set;}
     public int PageSize {get; set;}
     public int TotalRecords {get; set;}
     public string searchKey {get; set;}

     public AddTaxviewmodel AddTax {get; set;}
}

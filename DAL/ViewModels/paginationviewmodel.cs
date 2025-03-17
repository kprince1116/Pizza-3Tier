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

    //   public int Itemid { get; set; }

    public int Modifierid { get; set; }
    public int Categoryid { get; set; }

    // public string Itemname { get; set; } = null!;

    // public string? Itemtype { get; set; }

    // public int? Quantity { get; set; }

    // public decimal? Rate { get; set; }

    // public bool IsAvailable { get; set; }

    // public string? Image { get; set; }
    // public string? Description { get; set; }

    // public decimal? TaxPercentage { get; set; }

    // public bool? IsFavourite { get; set; }

    // public string? ShortCode { get; set; }

    // public bool IsDefaultTax { get; set; }

    // public virtual MenuCategory? Category { get; set; }

    // public virtual Unit? Unit { get; set; }

      
    // public int FromRec {get ; set;}

    // public int Torec{get; set;}
}

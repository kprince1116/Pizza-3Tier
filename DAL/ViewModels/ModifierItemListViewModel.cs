namespace Pizzashop.DAL.ViewModels;

public class ModifierItemListViewModel
{
     public IEnumerable<ModifierItemViewModel>? ModifierItemList { get; set; }
    public paginationviewmodel? Page { get; set; }
}

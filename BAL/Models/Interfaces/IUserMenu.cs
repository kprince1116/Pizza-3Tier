using DAL.Models;
using DAL.ViewModels;
using Pizzashop.DAL.ViewModels;

namespace BAL.Interfaces;

public interface IUserMenu
{
    public List<Categoryviewmodel> GetCategories();

    public List<Unit> GetUnits();
    public Task<paginationviewmodel> GetItemsByCategory(int id , int pageNo  , int pageSize, string search = "");

    // public Task<int> GetItemsCountByCategory(int categoryId);

    public Task<bool> AddCategory(Categoryviewmodel model);

    public Task<bool> GetModifierById(int id);

    public Task<bool> UpdateCategory(Categoryviewmodel model);

    public Task<bool> AddNewItem( menuviewmodel model);
    
    public Task<EditItemviewmodel> GetEditItem(int id);
    public Task<bool> EditItem(EditItemviewmodel Model);

    public Task<bool> GetItemForDeleteById(int id);

    public Task DeleteItemsAsync(List<int> itemList);
    // Task UpdateItemAsync(MenuItemsviewmodel existingitem);
    // public Task<ItemListviewmodel>  GetItemList(int categoryId,int pageNo,int pageSize,string search);

    // <!-- modifiers --!>
    public IEnumerable<ModifierGroupViewModel> GetAllModifierGroup();

    public Task<bool> AddModifier(ModifierGroupViewModel model);
  
    public Task<bool> UpdateModifier(ModifierGroupViewModel model);

    public Task<bool> GetModifierByIdForDelete(int id);

    public Task<ModifierItemListViewModel> GetItemsByModifier(int id ,  int pageNo  , int pageSize, string search );
}

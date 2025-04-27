using DAL.Models;
using DAL.ViewModels;
using Pizzashop.DAL.ViewModels;

namespace DAL.Interfaces;

public interface IUserMenuRepository
{

    List<MenuCategory> GetCategories();

    List<Unit> GetUnits();

    List<ModifierGroup> GetAllModifierGroups();

    List<ModifierItemViewModel> GetModifierItems();

    // List<ItemModifierGroupviewmodel> GetModifierItem();

    public Task<paginationviewmodel> GetItemsByCategory(int id,  int pageNo = 1 , int pageSize=3, string search = "");

    public Task<ModifierGroupViewModel> GetModifierItemById(int modifierId);

    public Task<ModifierItemListViewModel> GetItemsByExistingModifier(int pageNo = 1 , int pageSize=3, string search = "");

    public Task<int> GetCount(int categoryId);

    public Task<bool> AddCategory( menuviewmodel model);

    public Task<MenuCategory> GetUserByIdForDelete(int id);

    Task DeleteCategoryAsync(MenuCategory existingCategory);
    
    Task<bool> GetCategoryName(string CategoryName);

    public Task<MenuCategory> GetCategoryId(int id);

    Task UpdateCategoryAsync(MenuCategory category);

    Task AddNewItem(menuviewmodel model);

    public Task<EditItemviewmodel> GetEditItem(int id); 

    public Task<MenuItem> EditItemAvailabity(int id);

    Task UpdateItemAsync(MenuItem existingitem);

    public Task<MenuItem> GetExistingItem(int id);

    Task<bool> UpdateItem( EditItemviewmodel model);

    public Task<MenuItem> GetItemForDeleteById(int id);

    Task DeleteItemAsync(MenuItem existingitem);

    Task DeleteItems(List<int> itemList); 


    // public  Task<(List<MenuItemsviewmodel> items , int TotalRecords)> GetItemList(int categoryId,int pageNo,int pageSize,String search);

    //Modifiers

    public IEnumerable<ModifierGroup> GetAllModifierGroup();

    //ADD Modifier Group
    public Task<bool> AddModifier(menuviewmodel  model);

    public Task<ModifierGroup> GetModifierId(int id);

    public  Task<bool> UpdateModifierAsync(menuviewmodel model);

     public Task<ModifierGroup> GetModifierByIdForDelete(int id);

    Task DeleteModifierAsync( ModifierGroup existingModifier);

    public Task<ModifierItemListViewModel> GetItemsByModifierId(int id,int pageNo,int pageSize,string search);

    Task AddModifierItem(menuviewmodel  model);
    Task EditModifierItem(AddModifierViewModel  model);

    public Task<AddModifierViewModel> GetEditModifierItem(int id);

    public Task<Modifier> GetExistingModifier(int ModifierId);

    // Task UpdateModifierItemAsync( Modifier existingmodifier);

    public Task<Modifier> GetModifierItemForDeleteById(int id);

    Task DeleteModifierItemAsync( Modifier existingModifierItem);

    Task DeleteModifiers(List<int> modifierList,int modifiergroupId);

    Task<List<ModifierItemViewModel>> GetExistingModifierItems(int id);

    Task<List<ItemModifierGroupviewmodel>> GetAllModifierItemById(int id);
    
}

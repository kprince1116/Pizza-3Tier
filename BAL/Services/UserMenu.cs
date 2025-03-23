using BAL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pizzashop.DAL.ViewModels;

namespace BAL.Services;

public class UserMenu : IUserMenu
{
    private readonly IUserMenuRepository _userMenuRepository;

    private readonly PizzaShopContext _db;


    public UserMenu(IUserMenuRepository userMenuRepository,PizzaShopContext db)
    {
        _userMenuRepository = userMenuRepository;
         _db = db;
    }
    public List<Categoryviewmodel> GetCategories()
    {
        var categories = _userMenuRepository.GetCategories();
        var categoryViewModels = categories.Select(c => new Categoryviewmodel
        {
            CategoryId = c.Categoryid,
            CategoryName = c.Name,
            Description = c.Description
        }).ToList();

        return categoryViewModels;
    }

     public List<Unit> GetUnits()
     {
        var units = _userMenuRepository.GetUnits();
        return units;
     }

    public List<ModifierGroupViewModel> GetModifiers()
    {
        var modifiergroups = _userMenuRepository.GetAllModifierGroups();
        var ModifierGroupViewModel =  modifiergroups.Select(m => new ModifierGroupViewModel
        {
            ModifierId = m.ModifierGroupId,
            Name = m.Name,
            Description = m.Description
        }).ToList();
        return ModifierGroupViewModel;
    }

    public List<ModifierItemViewModel> GetModifierItems()
    {
        var modifierItems = _userMenuRepository.GetModifierItems();
        return modifierItems;
    }

    // public  List<ItemModifierGroupviewmodel> GetModifierItem()
    // {
    //     var modifierItems = _userMenuRepository.GetModifierItem();
    //     return modifierItems;
    // }
     public async Task<paginationviewmodel> GetItemsByCategory(int id , int pageNo = 1 , int pageSize=3 , string search = "")
     {
        var items = await _userMenuRepository.GetItemsByCategory(id,pageNo,pageSize,search);

        return items;
     }

      public  Task<ModifierGroupViewModel> GetModifierItemById(int modifierId)
      {
        return  _userMenuRepository.GetModifierItemById(modifierId);
      }

     public Task<ModifierItemListViewModel> GetItemsByExistingModifier( int pageNo  , int pageSize, string search = "")
     {
        var items = _userMenuRepository.GetItemsByExistingModifier(pageNo,pageSize,search);
        return items;
     }
    public async Task<int> GetItemsCountByCategory(int categoryId)
    {
        return await _userMenuRepository.GetCount(categoryId);
    }

     public async Task<bool> AddCategory(Categoryviewmodel model)
     {
         return await _userMenuRepository.AddCategory(model);
     }
     
    public async Task<bool> GetModifierById(int id)
    {
        var existingCategory = await _userMenuRepository.GetUserByIdForDelete(id);

        if(existingCategory == null)
        {
            return false;
        }
        existingCategory.IsDeleted = true;  

         await _userMenuRepository.DeleteCategoryAsync(existingCategory);

         return true;
    }

     public async Task<bool> GetItemForDeleteById(int id)
      {
        var existingitem = await _userMenuRepository.GetItemForDeleteById(id);

        if(existingitem == null)
        {
            return false;
        }
        existingitem.IsDeleted = true;  

         await _userMenuRepository.DeleteItemAsync(existingitem);

         return true;

      }


    public async Task<bool> UpdateCategory(Categoryviewmodel model)
    {
        var category = await _userMenuRepository.GetCategoryId(model.CategoryId);

        if(category == null)
        {
            return false;
        }

        category.Name = model.CategoryName;
        category.Description = model.Description;

        await _userMenuRepository.UpdateCategoryAsync(category);
        return true;
    }
    
     public async Task<bool> AddNewItem(menuviewmodel model)
     {
        if(model.Additem==null)
        {
            return false;
        }
        await _userMenuRepository.AddNewItem(model);
        return true;
     }

     public async Task<EditItemviewmodel> GetEditItem(int id)
     {
        return await _userMenuRepository.GetEditItem(id);
     }

  
      public async Task<bool> EditItem(EditItemviewmodel model)
      {
         await _userMenuRepository.UpdateItem(model);
         return true;
      }

      public async Task DeleteItemsAsync(List<int> itemList)
        {
            await _userMenuRepository.DeleteItems(itemList);
        }

        
    
    //Modifiers

    public IEnumerable<ModifierGroupViewModel> GetAllModifierGroup()
    {
        IEnumerable<ModifierGroup> modifiergroups = _userMenuRepository.GetAllModifierGroup();
        return modifiergroups.Select(m => new ModifierGroupViewModel
        {
            ModifierId = m.ModifierGroupId,
            Name = m.Name,
            Description = m.Description
        });
    }

    //Add Modifier Group

     public Task<bool> AddModifier(ModifierGroupViewModel model)
     {
         return _userMenuRepository.AddModifier(model);
     }

     //Update Modifier Group

     public async Task<bool> UpdateModifier(ModifierGroupViewModel model)
     {
        return await _userMenuRepository.UpdateModifierAsync( model);
    
     }

     //Delete Modifier Group
     public async Task<bool> GetModifierByIdForDelete(int id)
     {
          var existingModifier = await  _userMenuRepository.GetModifierByIdForDelete(id);

          if(existingModifier == null)
          {
              return false;
          }

          existingModifier.IsDeleted = true;

          await _userMenuRepository.DeleteModifierAsync(existingModifier);
          return true;
     }

     public Task<ModifierItemListViewModel> GetItemsByModifier(int id ,  int pageNo  , int pageSize, string search )
     {
            var item = _userMenuRepository.GetItemsByModifierId(id,pageNo,pageSize,search);
            return item;
     }

     //add modifier

      public async Task<bool> AddModifierItem(menuviewmodel model)
      {
        if(model.AddModifier==null)
        {
            return false;
        }
        await _userMenuRepository.AddModifierItem(model);
        return true;
         
      }

         public Task<AddModifierViewModel> GetEditModifierItem(int id)
     {
            return _userMenuRepository.GetEditModifierItem(id);
     }

     public  async Task<bool> EditModifierItem(AddModifierViewModel model)
     {
        if(model==null)
        {
            return false;
        }
       await _userMenuRepository.EditModifierItem(model);
        return true;
     }

     public async Task<bool> GetModifierItemForDeleteById(int id , int  modifiergroupId)
        {
            var existingModifierItem =  _db.Modifiermappings.Where(m => m.ModifierId == id && m.ModifierGroupId == modifiergroupId ).FirstOrDefault();

            if(existingModifierItem == null)
            {
                return false;
            }
            existingModifierItem.IsDeleted = true;

                try{
                    _db.Modifiermappings.Update(existingModifierItem);
                    _db.SaveChanges();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
             

            // await _userMenuRepository.DeleteModifierItemAsync(existingModifierItem);
            return true;
        }

           public async Task DeleteModifiersAsync(List<int> modifierList,int modifiergroupId)
           {
              await _userMenuRepository.DeleteModifiers(modifierList,modifiergroupId);
           }

        public async Task<List<ModifierItemViewModel>> GetExistingModifierItems(int id)
            {
                return await _userMenuRepository.GetExistingModifierItems(id);
            }

    public Task<List<ItemModifierGroupviewmodel>> GetAllModifierItemById(int id)
    {
        return _userMenuRepository.GetAllModifierItemById(id);
    }
}

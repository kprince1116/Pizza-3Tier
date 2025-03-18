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
     public async Task<paginationviewmodel> GetItemsByCategory(int id , int pageNo = 1 , int pageSize=3 , string search = "")
     {
        var items = await _userMenuRepository.GetItemsByCategory(id,pageNo,pageSize,search);

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
        var existingitem = await _userMenuRepository.GetExistingItem(model.Itemid);

         if(existingitem == null)
        {
            return false;
        }
        existingitem.Categoryid = model.Categoryid;
        existingitem.Itemid = model.Itemid;
        existingitem.Itemname = model.Itemname;
        existingitem.Description = model.Description;
        existingitem.Quantity = model.Quantity;
        existingitem.Rate = model.Rate;
        existingitem.IsAvailable = model.IsAvailable;
        existingitem.ShortCode = model.ShortCode;
        existingitem.Unitid = model.UnitId;
        existingitem.TaxPercentage = model.TaxPercentage;
        existingitem.Image = model.Image;

         await _userMenuRepository.UpdateItemAsync(existingitem);
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
        var modifier = await _userMenuRepository.GetModifierId(model.ModifierId);
        
        if(modifier == null)
        {
            return false;
        }

        modifier.Name = model.Name; 
        modifier.Description = model.Description;

        await _userMenuRepository.UpdateModifierAsync(modifier);

        return true;

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

         public Task<EditModifierViewModel> GetEditModifierItem(int id)
     {
            return _userMenuRepository.GetEditModifierItem(id);
     }

     public  async Task<bool> EditModifierItem( EditModifierViewModel model)
     {
        Modifier existingmodifier =   _db.Modifiers.Where(m => m.Modifierid == model.ModifierId).FirstOrDefault();

         if(existingmodifier == null)
        {
            return false;
        }

        existingmodifier.Modifiername = model.ModifierName;
        existingmodifier.Description = model.Description;
        existingmodifier.Rate = model.Rate; 
        existingmodifier.Quantity = model.Quantity;
        existingmodifier.Unitid = model.UnitId;

      
        try{
            _db.SaveChanges();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    
          return true;
     }

     public async Task<bool> GetModifierItemForDeleteById(int id)
        {
            var existingModifierItem =  _db.Modifiers.Where(m => m.Modifierid == id).FirstOrDefault();

            if(existingModifierItem == null)
            {
                return false;
            }
            existingModifierItem.IsDeleted = true;

                try{
                    _db.SaveChanges();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
             

            // await _userMenuRepository.DeleteModifierItemAsync(existingModifierItem);
            return true;
        }

           public async Task DeleteModifiersAsync(List<int> modifierList)
           {
              await _userMenuRepository.DeleteModifiers(modifierList);
           }

}

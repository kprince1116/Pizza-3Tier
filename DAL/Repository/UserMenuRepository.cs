using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Pizzashop.DAL.ViewModels;

namespace DAL.Repository;

public class UserMenuRepository : IUserMenuRepository
{

    private readonly PizzaShopContext _db;

    public UserMenuRepository(PizzaShopContext db)
    {
        _db = db;
    }
    public List<MenuCategory> GetCategories()
    {
        return _db.MenuCategories.Where(u => u.IsDeleted == false).ToList();
    }

    public List<ModifierGroup> GetAllModifierGroups()
    {
        return _db.ModifierGroups.Where(u => u.IsDeleted == false).ToList();
    }

    public List<ModifierItemViewModel> GetModifierItems()
    {
        return _db.Modifiers.Where(u => u.IsDeleted == false).Select(u => new ModifierItemViewModel
        {
            ModifierItemId = u.Modifierid,
            // ModifierId = u.ModifierGroupId,
            Name = u.Modifiername,
            Rate = u.Rate,
            Quantity = u.Quantity,
            // Description = u.Description
        }).ToList();
    }
    public List<Unit> GetUnits()
    {
        return _db.Units.ToList();
    }

    public async Task<paginationviewmodel> GetItemsByCategory(int id, int pageNo = 1, int pageSize = 3, string search = "")
    {
        var item = _db.MenuItems
            .Where(u => !u.IsDeleted.HasValue || !u.IsDeleted.Value && u.Categoryid == id &&
                        (u.Itemname.ToLower().Contains(search.ToLower()) || u.Description.ToLower().Contains(search.ToLower())))
            .Select(u => new MenuItemsviewmodel
            {
                Itemid = u.Itemid,
                Categoryid = u.Categoryid,
                Itemname = u.Itemname,
                Itemtype = u.Itemtype,
                Description = u.Description,
                Rate = u.Rate,
                Quantity = u.Quantity,
                IsAvailable = (bool)u.IsAvailable,
                Image = u.Image,
            });

        var totalRecords = item.Count();

        var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

        List<MenuItemsviewmodel> items = await item.Skip((pageNo - 1) * pageSize).Take(pageSize).OrderBy(u => u.Itemid).ToListAsync();

        return new paginationviewmodel
        {
            Items = items,
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            CurrentPage = pageNo,
            PageSize = pageSize
        };
    }

    //existing modifier

    public async Task<ModifierItemListViewModel> GetItemsByExistingModifier(int pageNo = 1, int pageSize = 3, string search = "")
    {
        var item = _db.Modifiers
           .Where(u => !u.IsDeleted.HasValue || !u.IsDeleted.Value && (u.Modifiername.ToLower().Contains(search.ToLower()) || u.Description.ToLower().Contains(search.ToLower())))
           .Select(u => new ModifierItemViewModel
           {
               ModifierItemId = u.Modifierid,
               Name = u.Modifiername,
               Unit = u.Unitid,
               Unitname = _db.Units.Where(u => u.Unitid == u.Unitid).Select(u => u.Unitname).FirstOrDefault(),
               Rate = u.Rate,
               Quantity = u.Quantity
           });

        var totalRecords = item.Count();

        var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

        List<ModifierItemViewModel> items = await item.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();

        return new ModifierItemListViewModel
        {
            ModifierItemList = items,
            Page = new paginationviewmodel

            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = pageNo,
                PageSize = pageSize
            }
        };

    }

    public Task<int> GetCount(int categoryId)
    {
        var query = _db.MenuItems;

        return query.CountAsync();
    }

    public async Task<bool> AddCategory(Categoryviewmodel model)
    {
        var category = new MenuCategory
        {
            Name = model.CategoryName,
            Description = model.Description,
        };

        _db.MenuCategories.Add(category);
        bool success = await _db.SaveChangesAsync() > 0;
        return success;
    }

    public Task<MenuCategory> GetUserByIdForDelete(int id)
    {
        return _db.MenuCategories.FirstOrDefaultAsync(m => m.Categoryid == id);
    }

    public async Task DeleteCategoryAsync(MenuCategory existingCategory)
    {
        _db.MenuCategories.Update(existingCategory);
        await _db.SaveChangesAsync();
    }

    public Task<MenuCategory> GetCategoryId(int id)
    {
        return _db.MenuCategories.FirstOrDefaultAsync(c => c.Categoryid == id);
    }

    public async Task UpdateCategoryAsync(MenuCategory category)
    {
        _db.MenuCategories.Update(category);
        await _db.SaveChangesAsync();
    }

    public async Task AddNewItem(menuviewmodel model)
    {

        var item = new MenuItem
        {
            Categoryid = model.Additem.Categoryid,
            Itemname = model.Additem.Itemname,
            Itemtype = model.Additem.Itemtype,
            Quantity = model.Additem.Quantity,
            Rate = model.Additem.Rate,
            IsAvailable = model.Additem.IsAvailable,
            Image = model.Additem.Image,
            Description = model.Additem.Description,
            TaxPercentage = model.Additem.TaxPercentage,
        };


        _db.MenuItems.Update(item);
        await _db.SaveChangesAsync();
    }

   
    public async Task<EditItemviewmodel> GetEditItem(int id)
    {
        return await _db.MenuItems.Where(u => u.Itemid == id).Select(u => new EditItemviewmodel
        {
            Categoryid = u.Categoryid,
            Itemname = u.Itemname,
            Itemtype = u.Itemtype,
            Quantity = u.Quantity,
            Rate = u.Rate,
            IsAvailable = (bool)u.IsAvailable,
            Image = u.Image,
            Description = u.Description,
            TaxPercentage = u.TaxPercentage,
            Categories = _db.MenuCategories.ToList(),
            units = _db.Units.ToList(),
            UnitId = u.Unitid,
            ShortCode = u.ShortCode,
        }
        ).FirstOrDefaultAsync();
    }


    public async Task<MenuItem> GetExistingItem(int id)
    {
        return await _db.MenuItems.FirstOrDefaultAsync(u=>u.Itemid == id);
    }


    public async Task UpdateItemAsync(MenuItem existingitem)
    {
        _db.MenuItems.Update(existingitem);
        await _db.SaveChangesAsync();
    }

    public async Task<MenuItem> GetItemForDeleteById(int id)
    {
        return await _db.MenuItems.FirstOrDefaultAsync(m => m.Itemid == id);
    }


    public async Task DeleteItemAsync(MenuItem existingitem)
    {
        _db.MenuItems.Update(existingitem);
        await _db.SaveChangesAsync();
    }


    public async Task DeleteItems(List<int> itemList)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            MenuItem item = _db.MenuItems.Where(e => e.Itemid == itemList[i]).FirstOrDefault();
            item.IsDeleted = true;

        }
        // var items = await _db.MenuItems.Where(item => itemList.Contains(item.Itemid)).ToListAsync();
        // _db.MenuItems.RemoveRange(items);
        await _db.SaveChangesAsync();
    }


    //Modiers
    public IEnumerable<ModifierGroup> GetAllModifierGroup()
    {
        return _db.ModifierGroups.Where(u => u.IsDeleted == false).ToList().OrderBy(u => u.ModifierGroupId);
    }

    //Add Modifier
    public async Task<bool> AddModifier(ModifierGroupViewModel model)
    {
        ModifierGroup exisingModifiergroup = await _db.ModifierGroups.Where(u => u.Name == model.Name && (!u.IsDeleted.HasValue || !u.IsDeleted.Value) && u.ModifierGroupId != model.ModifierId).FirstOrDefaultAsync();

        if (exisingModifiergroup != null && exisingModifiergroup.IsDeleted == false)
        {
            return false;
        }

        if (exisingModifiergroup != null && exisingModifiergroup.IsDeleted == true)
        {
            exisingModifiergroup.IsDeleted = false;
            await _db.SaveChangesAsync();
            return true;
        }

        try
        {
            ModifierGroup modifierGroup = new ModifierGroup
            {
                Name = model.Name,
                Description = model.Description
            };

            _db.ModifierGroups.Add(modifierGroup);
            bool success = await _db.SaveChangesAsync() > 0;

            if(await AddModifierItem(modifierGroup.ModifierGroupId , model.ModifierItemList))
            {
            return success;
            }
            else
            {
                return false;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }


    }

    public async Task<ModifierGroup> GetModifierId(int id)
    {
        return await _db.ModifierGroups.FirstOrDefaultAsync(m => m.ModifierGroupId == id);
    }

    //edit Modifier
    public async Task<bool> UpdateModifierAsync(ModifierGroupViewModel model)
    {
         ModifierGroup exisingModifiergroup = await _db.ModifierGroups.Where(u => u.Name == model.Name && (!u.IsDeleted.HasValue || !u.IsDeleted.Value) && u.ModifierGroupId != model.ModifierId).FirstOrDefaultAsync();

        if (exisingModifiergroup != null && exisingModifiergroup.IsDeleted == false)
        {
            return false;
        }

        if (exisingModifiergroup != null && exisingModifiergroup.IsDeleted == true)
        {
            exisingModifiergroup.IsDeleted = false;
            await _db.SaveChangesAsync();
            return true;
        }

        try
        {
             ModifierGroup? modifiergroup = _db.ModifierGroups.FirstOrDefault(m => m.ModifierGroupId == model.ModifierId);
            
            modifiergroup.Name = model.Name;
            modifiergroup.Description = model.Description;

            _db.ModifierGroups.Update(modifiergroup);
            bool success = await _db.SaveChangesAsync() > 0;

            if(await AddModifierItem(modifiergroup.ModifierGroupId , model.ModifierItemList))
            {
            return success;
            }
            else
            {
                return false;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

        // _db.ModifierGroups.Update(category);
        // await _db.SaveChangesAsync();
    }

    public async Task<ModifierGroup> GetModifierByIdForDelete(int id)
    {
        return await _db.ModifierGroups.FirstOrDefaultAsync(m => m.ModifierGroupId == id);
    }

    public async Task DeleteModifierAsync(ModifierGroup existingModifier)
    {
        _db.ModifierGroups.Update(existingModifier);
        await _db.SaveChangesAsync();
    }

    public async Task<ModifierItemListViewModel> GetItemsByModifierId(int id, int pageNo, int pageSize, string search)
    {
 
        var item = _db.Modifiermappings.Include(m => m.Modifier).Where(u => u.ModifierGroupId == id && u.IsDeleted == false && (u.Modifier.Modifiername.ToLower().Contains(search.ToLower()) || u.Modifier.Description.ToLower().Contains(search.ToLower())) ).Select(u => new ModifierItemViewModel
        {
            ModifierGroupId = (int)u.ModifierGroupId,
            ModifierItemId = (int)u.ModifierId,
            Name = u.Modifier.Modifiername,
            Rate = u.Modifier.Rate,
            Quantity = u.Modifier.Quantity,
            Unit = u.Modifier.Unitid,
            Unitname = _db.Units.Where(u => u.Unitid == u.Unitid).Select(u => u.Unitname).FirstOrDefault(),
        });

        // var item = _db.Modifiers
        //     .Where(u => !u.IsDeleted.HasValue || !u.IsDeleted.Value && u.ModifierGroupId == id && (u.Modifiername.ToLower().Contains(search.ToLower()) || u.Description.ToLower().Contains(search.ToLower())))
        //     .Select(u => new ModifierItemViewModel
        //     {
        //         ModifierItemId = u.Modifierid,
        //         Name = u.Modifiername,
        //         Unit = u.Unitid,
        //         Unitname = _db.Units.Where(u => u.Unitid == u.Unitid).Select(u => u.Unitname).FirstOrDefault(),
        //         Rate = u.Rate,
        //         Quantity = u.Quantity
        //     });
        


        var totalRecords = item.Count();

        var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

        List<ModifierItemViewModel> items = await item.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();

        return new ModifierItemListViewModel
        {
            ModifierItemList = items,
            Page = new paginationviewmodel

            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = pageNo,
                PageSize = pageSize
            }
        };

    }

    public async Task AddModifierItem(menuviewmodel model)
    {
        var modifier = new Modifier
        {
            ModifierGroupId = model.AddModifier.ModifierGroupId,
            Modifiername = model.AddModifier.ModifierName,
            Rate = model.AddModifier.Rate,
            Quantity = model.AddModifier.Quantity,
            Description = model.AddModifier.Description
        };

        _db.Modifiers.Add(modifier);
        await _db.SaveChangesAsync();
        await SaveModifierItem(modifier.Modifierid, model.AddModifier);
    }

     public async Task EditModifierItem(AddModifierViewModel model)
    {
         Modifier existingmodifier =   _db.Modifiers.Where(m => m.Modifierid == model.ModifierId).FirstOrDefault();


        existingmodifier.Modifiername = model.ModifierName;
        existingmodifier.Description = model.Description;
        existingmodifier.Rate = model.Rate; 
        existingmodifier.Quantity = model.Quantity;
        existingmodifier.Unitid = model.UnitId;

      
        try{
             _db.Modifiers.Update(existingmodifier);
            _db.SaveChanges();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
         await SaveModifierItem(existingmodifier.Modifierid, model);
          
    }
    public async Task SaveModifierItem(int modifierId, AddModifierViewModel model)
    {
         Modifiermapping? existingOne = await _db.Modifiermappings
                                            .Where(im => im.ModifierGroupId == model.ModifierGroupId && im.ModifierId == modifierId && im.IsDeleted==false)
                                            .FirstOrDefaultAsync();

        if (existingOne != null)
        {
           
            _db.Modifiermappings.Update(existingOne);
            await _db.SaveChangesAsync();
            return;
        }

       Modifiermapping modifiergroupitemmap = new Modifiermapping
        {
            ModifierGroupId = model.ModifierGroupId,
            ModifierId = modifierId,

        };
        _db.Modifiermappings.Add(modifiergroupitemmap);
        await _db.SaveChangesAsync();
    }

    public async Task<AddModifierViewModel> GetEditModifierItem(int id)
    {
        return await _db.Modifiers.Where(u => u.Modifierid == id).Select(u => new AddModifierViewModel
        {
            units = _db.Units.ToList(),
            modifiers = _db.ModifierGroups.ToList(),
            ModifierGroupId = u.ModifierGroupId,
            ModifierName = u.Modifiername,
            Rate = u.Rate,
            Quantity = u.Quantity,
            Description = u.Description,
            UnitId = u.Unitid,
        }).FirstOrDefaultAsync();
    }

    public async Task<Modifier> GetExistingModifier(int ModifierId)
    {
        try
        {
            return await _db.Modifiers.FirstOrDefaultAsync(m => m.Modifierid == ModifierId);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }

    }

    // public async Task  UpdateModifierItemAsync( Modifier existingmodifier)
    // {
    //      _db.Modifiers.Update(existingmodifier);
    //     await  _db.SaveChangesAsync();
    // }

    public Task<Modifier> GetModifierItemForDeleteById(int id)
    {
        return _db.Modifiers.FirstOrDefaultAsync(m => m.Modifierid == id);
    }

    public async Task DeleteModifierItemAsync(Modifier existingModifierItem)
    {
        // _db.Modifiers.Update(existingModifierItem);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteModifiers(List<int> modifierList ,int modifiergroupId)
    {
        for (int i = 0; i < modifierList.Count; i++)
        {
            Modifiermapping modifier = _db.Modifiermappings.Where(e => e.ModifierId == modifierList[i] && e.ModifierGroupId == modifiergroupId ).FirstOrDefault();
            modifier.IsDeleted = true;
            // _db.Modifiermappings.Update(modifier);
            
        }
        await _db.SaveChangesAsync();
    }

    public async Task<List<ModifierItemViewModel>> GetExistingModifierItems(int id)
    {
        var modifierList = await _db.Modifiermappings.Include(m=>m.Modifier).Where(u => u.ModifierGroupId == id && u.IsDeleted == false).Select(u => new ModifierItemViewModel
        {
            ModifierItemId =(int) u.ModifierId,
            Name = u.Modifier.Modifiername, 
            Rate = u.Modifier.Rate,
            Quantity = u.Modifier.Quantity,
            Unit = u.Modifier.Unitid,
            Unitname = _db.Units.Where(u => u.Unitid == u.Unitid).Select(u => u.Unitname).FirstOrDefault(),
        
        }).ToListAsync();

        return modifierList;

        // return await _db.Modifiers.Where(u => u.ModifierGroupId == id && u.IsDeleted == false).Select(u => new ModifierItemViewModel
        // {
        //     ModifierItemId = u.Modifierid,
        //     Name = u.Modifiername,
        //     Rate = u.Rate,
        //     Quantity = u.Quantity,
        //     Unit = u.Unitid,
        //     Unitname = _db.Units.Where(u => u.Unitid == u.Unitid).Select(u => u.Unitname).FirstOrDefault(),
        // }
        // ).ToListAsync();
    }

    public async Task<bool> AddModifierItem(int ModifierGroupId , List<ModifierItemViewModel> ModifierItemList)
    {
        foreach (var item in ModifierItemList)
            {
                Modifiermapping? existingOne = await _db.Modifiermappings
                .Where(mi => mi.ModifierGroupId == ModifierGroupId && mi.ModifierId == item.ModifierItemId && mi.IsDeleted==false)
                .FirstOrDefaultAsync();

                if (existingOne != null)
                {
                    continue;
                }
                Modifiermapping newModifierItemMap = new Modifiermapping
                {
                    ModifierGroupId = ModifierGroupId,
                    ModifierId = item.ModifierItemId,
                };

                await _db.Modifiermappings.AddAsync(newModifierItemMap);
                await _db.SaveChangesAsync();
            }
            return true;
    }


}

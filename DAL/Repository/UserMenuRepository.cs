using System.Data.Common;
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
            Name = u.Modifiername,
            Rate = u.Rate,
            Quantity = u.Quantity,
        }).OrderBy(u=>u.ModifierItemId).ToList();
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
            }).OrderBy(u=>u.Itemid);

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

    public async Task<ModifierGroupViewModel> GetModifierItemById(int modifierId)
    {
        ModifierGroupViewModel? modifiergroup = await _db.ModifierGroups.Where(m => m.ModifierGroupId == modifierId).
                                    Include(m => m.Modifiermappings).ThenInclude(m=>m.Modifier).
                                    Select(m => new ModifierGroupViewModel
                                    {
                                        ModifierId = m.ModifierGroupId,
                                        Name = m.Name,
                                        Description = m.Description,
                                        ModifierItemList = m.Modifiermappings.Where(m=>m.ModifierGroupId == modifierId && m.IsDeleted == false ).Select(i => new ModifierItemViewModel
                                        {
                                            ModifierItemId = i.Modifier.Modifierid,
                                            Name = i.Modifier.Modifiername,
                                            Rate = i.Modifier.Rate,
                                            Quantity = i.Modifier.Quantity,
                                        }).ToList()
                                    }).FirstOrDefaultAsync();

        return modifiergroup;
    }

    //existing modifier
    public async Task<ModifierItemListViewModel> GetItemsByExistingModifier(int pageNo = 1, int pageSize = 3, string search = "")
    {
        var item = _db.Modifiers.Include(u=>u.Unit)
           .Where(u => u.IsDeleted == false)
           .Select(u => new ModifierItemViewModel
           {
               ModifierItemId = u.Modifierid,
               Name = u.Modifiername,
               Unit = u.Unitid,
               Unitname = u.Unit.Unitname,
               Rate = u.Rate,
               Quantity = u.Quantity
           });

          if (!string.IsNullOrEmpty(search))
        {
            var lowerSearchQuery = search.ToLower();
            item = item.Where(o => o.Name.ToLower().Contains(lowerSearchQuery) ||
                                   o.Quantity.ToString().ToLower().Contains(lowerSearchQuery) ||
                                   o.Unitname.ToLower().Contains(lowerSearchQuery));
        }

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

    public async Task<bool> AddCategory(menuviewmodel model)
    {
        var category = new MenuCategory
        {
            Name = model.AddCategory.CategoryName,
            Description = model.AddCategory.Description,
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
            Description = model.Additem.Description,
            TaxPercentage = model.Additem.TaxPercentage,
        };

        if(model.Additem.ItemImage !=null)
        {
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string filename = $"{Guid.NewGuid()}_{model.Additem.ItemImage.FileName}";
            string filepath = Path.Combine(uploadsFolder, filename);

            using (FileStream fileStream = new FileStream(filepath, FileMode.Create))
            {
                await model.Additem.ItemImage.CopyToAsync(fileStream);
            }

            item.Image = $"/uploads/{filename}"; 
        }

        _db.MenuItems.Update(item);
        await _db.SaveChangesAsync();

        await AddItemModifier(item.Itemid, model.Additem.ItemModifierList);
        
    }

     public async Task<bool>  AddItemModifier(int itemId, List<ItemModifierGroupviewmodel> itemModifierList)
    {
        List<int> exisingModifierIds = await _db.Itemmodifiergroups
                                        .Where(im => im.Itemid == itemId && im.IsDeleted == false )
                                        .Select(mi => (int)mi.Modifiergroupid)
                                        .ToListAsync();

        List<int> newModifierGroupIds = itemModifierList.Select(m => m.ModifierGroupId).ToList();
        List<int> toBeRemoved = exisingModifierIds.Except(newModifierGroupIds).ToList();

        try
        {
            if (toBeRemoved.Count > 0)
            {
                foreach (var deleteItem in toBeRemoved)
                {
                    await DeleteSelectModifierGroups(itemId,deleteItem);
                }
            }
            
            foreach(var item in itemModifierList)
            {
                Itemmodifiergroup? existingOne = await _db.Itemmodifiergroups.Where(i => i.Itemid == itemId && i.Modifiergroupid == item.ModifierGroupId && i.IsDeleted == false)
                .FirstOrDefaultAsync();

                if(existingOne != null)
                {
                    existingOne.Minallowed = item.MinAllowed;
                    existingOne.Maxallowed = item.MaxAllowed;
                    _db.Itemmodifiergroups.Update(existingOne);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    Itemmodifiergroup newItemModifer = new Itemmodifiergroup{
                        Itemid = itemId,
                        Modifiergroupid = item.ModifierGroupId,
                        Minallowed = item.MinAllowed,
                        Maxallowed = item.MaxAllowed
                    };

                    await _db.Itemmodifiergroups.AddAsync(newItemModifer);
                    await _db.SaveChangesAsync();
                }
            }
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in Modifier Item", ex.Message);
            return false;
        }
    }

    public async Task<bool> DeleteSelectModifierGroups(int Itemid,int id)
    {
        try
        {
            Itemmodifiergroup? existingOne = await _db.Itemmodifiergroups
                                            .Where(im => im.Modifiergroupid == id && im.Itemid == Itemid && im.IsDeleted == false)
                                            .FirstOrDefaultAsync();

            if(existingOne == null)
            {
                return false;
            }

            existingOne.IsDeleted = true;

            _db.Itemmodifiergroups.Update(existingOne);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Deleting Selected Modifier Group", ex.Message);
            return false;
        }
    }

    public async Task<MenuItem> EditItemAvailabity(int id)
    {
        return await _db.MenuItems.FirstOrDefaultAsync(u=>u.Itemid == id);
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


    public async Task<bool> UpdateItem(EditItemviewmodel model)
    {
        var existingitem = await _db.MenuItems.FirstOrDefaultAsync(u=>u.Itemid == model.Itemid);

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

        
        if(model.ItemImage !=null)
        {
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string filename = $"{Guid.NewGuid()}_{model.ItemImage.FileName}";
            string filepath = Path.Combine(uploadsFolder, filename);

            using (FileStream fileStream = new FileStream(filepath, FileMode.Create))
            {
                await model.ItemImage.CopyToAsync(fileStream);
            }

            existingitem.Image = $"/uploads/{filename}"; 
        }
        

        _db.MenuItems.Update(existingitem);
        await _db.SaveChangesAsync();

        await AddItemModifier(existingitem.Itemid, model.ItemModifierList);

        return true;
        
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
   
        await _db.SaveChangesAsync();
    }


    //Modiers
    public IEnumerable<ModifierGroup> GetAllModifierGroup()
    {
        return _db.ModifierGroups.Where(u => u.IsDeleted == false).ToList().OrderBy(u => u.ModifierGroupId);
    }

    //Add Modifier
    public async Task<bool> AddModifier(menuviewmodel model)
    {



        ModifierGroup exisingModifiergroup = await _db.ModifierGroups.Where(u => u.Name == model.AddModifierGroup.Name && (!u.IsDeleted.HasValue || !u.IsDeleted.Value) && u.ModifierGroupId != model.AddModifierGroup.ModifierId).FirstOrDefaultAsync();

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
                Name = model.AddModifierGroup.Name,
                Description = model.AddModifierGroup.Description
            };

            _db.ModifierGroups.Add(modifierGroup);
            bool success = await _db.SaveChangesAsync() > 0;

            if(await AddModifierItem(modifierGroup.ModifierGroupId , model.AddModifierGroup.ModifierItemList))
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
    public async Task<bool> UpdateModifierAsync(menuviewmodel model)
    {
         ModifierGroup exisingModifiergroup = await _db.ModifierGroups.Where(u => u.Name == model.AddModifierGroup.Name && (!u.IsDeleted.HasValue || !u.IsDeleted.Value) && u.ModifierGroupId != model.AddModifierGroup.ModifierId).FirstOrDefaultAsync();

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
             ModifierGroup? modifiergroup = _db.ModifierGroups.FirstOrDefault(m => m.ModifierGroupId == model.AddModifierGroup.ModifierId);
            
            modifiergroup.Name = model.AddModifierGroup.Name;
            modifiergroup.Description = model.AddModifierGroup.Description;

            _db.ModifierGroups.Update(modifiergroup);
            bool success = await _db.SaveChangesAsync() > 0;

            if(await AddModifierItem(modifiergroup.ModifierGroupId , model.AddModifierGroup.ModifierItemList))
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
 
        var item = _db.Modifiermappings.Include(m => m.Modifier).ThenInclude(u=>u.Unit).Where(u => u.ModifierGroupId == id && u.IsDeleted == false && (u.Modifier.Modifiername.ToLower().Contains(search.ToLower()) || u.Modifier.Description.ToLower().Contains(search.ToLower())) ).Select(u => new ModifierItemViewModel
        {
            ModifierGroupId = (int)u.ModifierGroupId,
            ModifierItemId = (int)u.ModifierId,
            Name = u.Modifier.Modifiername,
            Rate = u.Modifier.Rate,
            Quantity = u.Modifier.Quantity,
            Unit = u.Modifier.Unit.Unitid,
            Unitname = u.Modifier.Unit.Unitname,
        }).OrderBy(u=>u.ModifierItemId);


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
            Description = model.AddModifier.Description,
            Unitid = model.AddModifier.UnitId
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

    public Task<Modifier> GetModifierItemForDeleteById(int id)
    {
        return _db.Modifiers.FirstOrDefaultAsync(m => m.Modifierid == id);
    }

     public async Task UpdateItemAsync(MenuItem existingitem)
    {
         _db.MenuItems.Update(existingitem);
        await _db.SaveChangesAsync();
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
            Modifiermapping modifier = _db.Modifiermappings.Where(e => e.ModifierId == modifierList[i] && e.ModifierGroupId == modifiergroupId && e.IsDeleted==false).FirstOrDefault();
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

    }

    public async Task<bool> AddModifierItem(int ModifierGroupId , List<ModifierItemViewModel> ModifierItemList)
    {

        List<int> exisingModifierIds = await _db.Modifiermappings
                                        .Where(im => im.ModifierGroupId == ModifierGroupId && im.IsDeleted == false )
                                        .Select(mi => (int)mi.ModifierId)
                                        .ToListAsync();

        List<int> newModifierGroupIds = ModifierItemList.Select(m => m.ModifierItemId).ToList();
        List<int> toBeRemoved = exisingModifierIds.Except(newModifierGroupIds).ToList();

        if (toBeRemoved.Count > 0)
            {
                foreach (var deleteItem in toBeRemoved)
                {
                    await DeleteSelectModifierItem(ModifierGroupId , deleteItem);
                }
            }
            

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

     public async Task<bool> DeleteSelectModifierItem(int ModifierGroupId,int ModifierId)
    {
        try
        {
            Modifiermapping? existingOne = await _db.Modifiermappings
                                            .Where(im => im.ModifierGroupId == ModifierGroupId && im.ModifierId == ModifierId && im.IsDeleted == false)
                                            .FirstOrDefaultAsync();

            if(existingOne == null)
            {
                return false;
            }

            existingOne.IsDeleted = true;

            _db.Modifiermappings.Update(existingOne);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Deleting Selected Modifier Group", ex.Message);
            return false;
        }
    }

    public async Task<List<ItemModifierGroupviewmodel>> GetAllModifierItemById(int id)
    {

        List<ItemModifierGroupviewmodel> model = await _db.Itemmodifiergroups.
            Include(mm => mm.Modifiergroup.Modifiermappings)
            .ThenInclude(mim => mim.Modifier)
            .Where(i => i.Itemid == id && i.IsDeleted ==false )
            .Select(i => new ItemModifierGroupviewmodel
            {
                ItemId =(int) i.Id, 
                ModifierGroupId =(int) i.Modifiergroupid,
                Name = i.Modifiergroup.Name,
                MinAllowed = i.Minallowed,
                MaxAllowed = i.Maxallowed,
                ModifierItemList = i.Modifiergroup.Modifiermappings
                .Where(m => m.IsDeleted == false && m.Modifier.IsDeleted == false)
                .Select(i => new ModifierItemViewModel
                {
                    ModifierItemId = i.Modifier.Modifierid,
                    Name = i.Modifier.Modifiername,
                    Rate = i.Modifier.Rate,
                    Quantity = i.Modifier.Quantity
                }).ToList()
            }).ToListAsync();
            return model;
        
    }


}

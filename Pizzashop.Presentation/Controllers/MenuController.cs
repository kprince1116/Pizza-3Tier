using System.Text.Json;
using BAL.Attributes;
using BAL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pizzashop.DAL.ViewModels;

namespace Pizzashop.Presentation.Controllers;

public class MenuController : Controller
{

    private readonly IUserMenu _userMenu;
    private readonly IHubContext<NotificationHub> _hubcontext;

    public MenuController(IUserMenu userMenu ,  IHubContext<NotificationHub> hubcontext)
    {
        _hubcontext = hubcontext;
        _userMenu = userMenu;
    }
    [Authorize(Roles = "Account_Manager,Admin")]
    [_AuthPermissionAttribute("Menu", ActionPermissions.CanView)]
    [HttpGet]
    [Route("Menu/Items")]
    public async Task<IActionResult> Items(int id, int pageNo = 1 , int pageSize=3)
    {
        var categories = _userMenu.GetCategories();
        var units = _userMenu.GetUnits();
        var modifiers = _userMenu.GetModifiers();
        var modifieritems = _userMenu.GetModifierItems();
        int selectedCategoryId = id != 0 ? id : categories.First().CategoryId;

        var items = await _userMenu.GetItemsByCategory(selectedCategoryId,  pageNo ,  pageSize);
        items.Categoryid = selectedCategoryId;
        AddItemviewmodel addItemModel = new AddItemviewmodel();
        addItemModel.ItemModifierList = new List<ItemModifierGroupviewmodel>();

        var viewModel = new menuviewmodel
        {
            Categories = categories,
            pagination = items,
            units = units,
            Modifiers = modifiers,
            ModifierItems = modifieritems,
            Additem = addItemModel
        };

        return View(viewModel);

    }

     #region Category CRUD
    [_AuthPermissionAttribute("Menu", ActionPermissions.CanAddEdit)]
    [HttpPost]
    public async Task<IActionResult> AddCategory(menuviewmodel model)
    {
        var isAdded = await _userMenu.AddCategory(model);
        if(isAdded){
            await _hubcontext.Clients.All.SendAsync("ItemMessage", "A Category was added.");
            return Json(new{success = true});
        }
        else{
            return Json (new{success = false});
        }    
    }

    [_AuthPermissionAttribute("Menu", ActionPermissions.CanAddEdit)]
    [HttpPost]
    public async Task<IActionResult> EditCategory(menuviewmodel model)
    {
        var isEdit = await _userMenu.UpdateCategory(model);
         if(isEdit){
            await _hubcontext.Clients.All.SendAsync("ItemMessage", "A Category was added.");
            return Json(new{success = true});
        }
        else{
            return Json (new{success = false});
        }    
    }

    [_AuthPermissionAttribute("Menu", ActionPermissions.CanDelete)]
    [HttpPost]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var existingCategory = await _userMenu.GetModifierById(id);

       if (existingCategory)
        {
            await _hubcontext.Clients.All.SendAsync("ItemMessage", "A Category was added.");
            TempData["DeleteCategorySuccess"] = true;
            return RedirectToAction("Items", "Menu");
        }
        else
        {
            return Content("error");
        }

    }
    #endregion

    #region Item CRUD

    [Route("Menu/ItemsByCategory")]
    public async Task<IActionResult> ItemsByCategory(int id , int pageNo = 1 , int pageSize=3, string searchKey = "" )
    {
        var categories = _userMenu.GetCategories();
        int selectedCategoryId = id != 0 ? id : categories.First().CategoryId;

        var items = await _userMenu.GetItemsByCategory(selectedCategoryId,pageNo,pageSize, searchKey);
        items.Categoryid = id;
        items.searchKey = searchKey;

        return PartialView("_ItemsPartial", items);
    }

     // ---------------------------------------------------- Add Item Modifier Select Group -----------------------------
    [HttpGet]
    public async Task<IActionResult> GetModifierItemById(int modifierGroupId , int itemId)
    {
        return PartialView("_modifierItemPartialView", await _userMenu.GetModifierItemById(modifierGroupId,itemId));
    }
   
    [_AuthPermissionAttribute("Menu", ActionPermissions.CanAddEdit)]
    [HttpPost]
    public async Task<IActionResult> Items(menuviewmodel model , string modifierItemList )
    {

        if (!string.IsNullOrEmpty(modifierItemList))
        {
            model.Additem.ItemModifierList = JsonSerializer.Deserialize<List<ItemModifierGroupviewmodel>>(modifierItemList);
        }

        var isAdded = await _userMenu.AddNewItem(model);

         if (isAdded)
        {
            await _hubcontext.Clients.All.SendAsync("ItemMessage", "A Category was added.");
            TempData["AddItemSuccess"] = true;
            return RedirectToAction("Items", "Menu");
        }
        else
        {
            return Content("error");
        }

    }

     public async Task<IActionResult> EditItem(int id)
    {
        
        var item =await _userMenu.GetEditItem(id);
        item.Itemid = id;

        item.ItemModifierList = await _userMenu.GetAllModifierItemById(id);
        item.ModifierGroupList = _userMenu.GetModifiers();

        return PartialView("_EditItemPartial",item);                                 
    }

    [_AuthPermissionAttribute("Menu", ActionPermissions.CanAddEdit)]
    [HttpPost]
    public async Task<IActionResult> EditItems(EditItemviewmodel model , string modifierItemListForEdit)
    {
        try
        {
            if (!string.IsNullOrEmpty(modifierItemListForEdit))
        {
            model.ItemModifierList = JsonSerializer.Deserialize<List<ItemModifierGroupviewmodel>>(modifierItemListForEdit);
        }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        var item = await _userMenu.EditItem(model);
          if(item){
            await _hubcontext.Clients.All.SendAsync("ItemMessage", "A Category was added.");
            return Json(new{success = true});
        }
        else{
            return Json (new{success = false});
        }                                
    }

    [_AuthPermissionAttribute("Menu", ActionPermissions.CanAddEdit)]
    [HttpPost]
    public async Task<IActionResult> EditItemAvailabity(int id , bool isAvailable)
    {
         var item =await _userMenu.EditItemAvailabity(id,isAvailable);
         if(item){
            await _hubcontext.Clients.All.SendAsync("ItemMessage", "A Category was added.");
            return Json( new { success = true, message = "hi"});
         }
         else{
            return Json( new { success = false, message = "hi"});
         }
         
    }

    [_AuthPermissionAttribute("Menu", ActionPermissions.CanDelete)]
    [HttpPost]
    public async Task<IActionResult> MenuListItemDelete(int id)
    {
        var isDelete =  await _userMenu.GetItemForDeleteById(id);

         if (isDelete)
        {
             await _hubcontext.Clients.All.SendAsync("ItemMessage", "A Category was added.");
            TempData["DeleteItemSuccess"] = true;
            return RedirectToAction("Items", "Menu");
        }
        else
        {
            return Content("error");
        }  
    }
    
    [_AuthPermissionAttribute("Menu", ActionPermissions.CanDelete)]
    [HttpPost]
    public async Task<IActionResult> DeleteCombine(List<int> itemList)
    {
        var isDelete = _userMenu.DeleteItemsAsync(itemList);
        if(isDelete !=null)
        {
        await _hubcontext.Clients.All.SendAsync("ItemMessage", "A Category was added.");
        return Json( new { success = true, message = "hi"});
        }
        else{
             return Json( new { success = false, message = "hi"});
        }
    }
     #endregion

    #region Modifier Group CRUD

    //Modifiers
    public IActionResult ModifiersTab(int id)
    {
        IEnumerable<ModifierGroupViewModel> model = _userMenu.GetAllModifierGroup();
        return PartialView("_ModifierTab", model);
    }

    //Add Modifier
     [_AuthPermissionAttribute("Menu", ActionPermissions.CanAddEdit)]
     [HttpPost]
    public async Task<IActionResult> AddModifier(menuviewmodel model , string modifierList)
    {
        if (!string.IsNullOrEmpty(modifierList))
        {
            
            model.AddModifierGroup.ModifierItemList = JsonSerializer.Deserialize<List<ModifierItemViewModel>>(modifierList);
        }

        var isAdded = await _userMenu.AddModifier(model);
        if (isAdded)
        {
            await _hubcontext.Clients.All.SendAsync("ModifierMessage", "A Category was added.");
            return Json( new { success = true});
        }
        else{
            return Json( new { success = false});
        }
        
        
    }
  
    // Edit Modifier
    [_AuthPermissionAttribute("Menu", ActionPermissions.CanAddEdit)]
    [HttpPost]
    public async Task<IActionResult> EditModifier(menuviewmodel model ,  string ExistingmodifierList )
    {
        try
        {
             if(!string.IsNullOrEmpty(ExistingmodifierList))
        {
        model.AddModifierGroup.ModifierItemList = JsonSerializer.Deserialize<List<ModifierItemViewModel>>(ExistingmodifierList);
        }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
       
        var isEdit = await _userMenu.UpdateModifier(model);
          if (isEdit)
        {
            await _hubcontext.Clients.All.SendAsync("ModifierMessage", "A Category was added.");
            return Json( new { success = true});
        }
        else{
            return Json( new { success = false});
        }
    }

    //Delete Modifier
    [_AuthPermissionAttribute("Menu", ActionPermissions.CanDelete)]
    [HttpPost]

    public async Task<IActionResult> DeleteModifer(int id)
    {
        var existingModifier = await _userMenu.GetModifierByIdForDelete(id);
        if (existingModifier)
        {
            await _hubcontext.Clients.All.SendAsync("ModifierMessage", "A Category was added.");
            TempData["DeleteModifierSuccess"] = true;
            return RedirectToAction("Items", "Menu");
        }
        else{
            return Content("errorsssss");
        }
    }
    #endregion
   
    #region Modifier Item CRUD

    public async Task<IActionResult> ItemsByModifier(int id , int pageNo = 1 , int pageSize=3, string searchKey = "" )
    {
        var items = await _userMenu.GetItemsByModifier(id,pageNo,pageSize, searchKey);
        items.Page.Modifierid = id;
        items.Page.searchKey = searchKey;

        return PartialView("_ModifierPartial", items);
    }

    [_AuthPermissionAttribute("Menu", ActionPermissions.CanAddEdit)]
    [HttpPost]
    public async Task<IActionResult> AddModifierItem(menuviewmodel model)
    {
        var isAdded = await _userMenu.AddModifierItem(model);
        if (isAdded)
        {
            await _hubcontext.Clients.All.SendAsync("ModifierMessage", "A Category was added.");
            return Json( new { success = true});
        }
        else
        {
             return Json( new { success = false});
        }
    }

    [_AuthPermissionAttribute("Menu", ActionPermissions.CanAddEdit)]
    public async Task<IActionResult> EditModifierItem(int editid)
    {
        var item =await _userMenu.GetEditModifierItem(editid);
        item.ModifierId = editid;

        return PartialView("_EditModifierItemPartial",item);                                 
    }

    [_AuthPermissionAttribute("Menu", ActionPermissions.CanAddEdit)]
    [HttpPost]
    public async Task<IActionResult> EditModifierItems(AddModifierViewModel model)
    {
        var modifiers = await _userMenu.EditModifierItem(model);
         if (modifiers)
        {
            await _hubcontext.Clients.All.SendAsync("ModifierMessage", "A Category was added.");
            return Json( new { success = true});
        }
        else
        {
             return Json( new { success = false});
        }
                                    
    }

    [_AuthPermissionAttribute("Menu", ActionPermissions.CanDelete)]
    [HttpPost]
    public async Task<IActionResult> DeleteModifierItem(int id , int modifiergroupId)
    {
        var existingmodifier =  await _userMenu.GetModifierItemForDeleteById(id,modifiergroupId);
         if (existingmodifier)
        {
            await _hubcontext.Clients.All.SendAsync("ModifierMessage", "A Category was added.");
            return Json( new { success = true});
        }
        else
        {
             return Json( new { success = false});
        }
    }

    [_AuthPermissionAttribute("Menu", ActionPermissions.CanDelete)]
    [HttpPost]
    public async Task<IActionResult> DeleteCombineForModifier(List<int> modifierList , int modifiergroupId)
    {
        var isDelete = _userMenu.DeleteModifiersAsync(modifierList , modifiergroupId);
        if(isDelete !=null)
        {
             await _hubcontext.Clients.All.SendAsync("ModifierMessage", "A Category was added.");
            return Json( new { success = true, message = "hi"});
        }
        else{
             return Json( new { success = false, message = "hi"});
        }
    }

      public async Task<IActionResult> ItemsByExistingModifier( int pageNo = 1 , int pageSize=3, string searchKey = "" )
    {
        var items = await _userMenu.GetItemsByExistingModifier(pageNo,pageSize, searchKey);

        return PartialView("_AddExistingModifier", items);
    }

    public async Task<IActionResult> GetExisistingsModifier(int id)
    {
        var items = await _userMenu.GetExistingModifierItems(id);
        return PartialView("_EditExistingModifier", items);
    }

    #endregion

}
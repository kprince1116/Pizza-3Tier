using System.Text.Json;
using BAL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pizzashop.DAL.ViewModels;

namespace Pizzashop.Presentation.Controllers;

public class MenuController : Controller
{

    private readonly IUserMenu _userMenu;

    public MenuController(IUserMenu userMenu)
    {
        _userMenu = userMenu;
    }

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

    [HttpPost]
    public async Task<IActionResult> AddCategory(menuviewmodel model)
    {

        var isAdded = await _userMenu.AddCategory(model);
        if (isAdded)
        {
            return RedirectToAction("Items", "Menu");
        }
        else
        {
            return Content("error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditCategory(menuviewmodel model)
    {
        await _userMenu.UpdateCategory(model);
        return RedirectToAction("Items", "Menu");
    }


    [HttpPost]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var existingCategory = await _userMenu.GetModifierById(id);

        return RedirectToAction("Items", "Menu");

    }

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
    public async Task<IActionResult> GetModifierItemById(int modifierId)
    {
        return PartialView("_modifierItemPartialView", await _userMenu.GetModifierItemById(modifierId));
    }


    [HttpPost]
    public async Task<IActionResult> Items(menuviewmodel model , string modifierItemList )
    {

        if (!string.IsNullOrEmpty(modifierItemList))
        {
            model.Additem.ItemModifierList = JsonSerializer.Deserialize<List<ItemModifierGroupviewmodel>>(modifierItemList);
        }

        var isAdded = await _userMenu.AddNewItem(model);


        return RedirectToAction("Index","Home");
    }

     public async Task<IActionResult> EditItem(int id)
    {
        
        var item =await _userMenu.GetEditItem(id);
        item.Itemid = id;

        item.ItemModifierList = await _userMenu.GetAllModifierItemById(id);
        item.ModifierGroupList = _userMenu.GetModifiers();

        return PartialView("_EditItemPartial",item);                                 
    }

    [HttpPost]
    public async Task<IActionResult> EditItems(EditItemviewmodel model , string modifierItemListForEdit)
    {

        if (!string.IsNullOrEmpty(modifierItemListForEdit))
        {
            model.ItemModifierList = JsonSerializer.Deserialize<List<ItemModifierGroupviewmodel>>(modifierItemListForEdit);
        }

        var item = await _userMenu.EditItem(model);
        return RedirectToAction("Items");                                  
    }

    [HttpPost]
    public async Task<IActionResult> EditItemAvailabity(int id , bool isAvailable)
    {
         var item =await _userMenu.EditItemAvailabity(id,isAvailable);
          return Json( new { success = true, message = "hi"});
    }

    [HttpPost]
    public async Task<IActionResult> MenuListItemDelete(int id)
    {
        await _userMenu.GetItemForDeleteById(id);

        return RedirectToAction("Index", "Home");
    }
    
    [HttpPost]
    public IActionResult DeleteCombine(List<int> itemList)
    {
        _userMenu.DeleteItemsAsync(itemList);
        return Json( new { success = true, message = "hi"});
    }
    
    //Modifiers
        public IActionResult ModifiersTab(int id)
    {
        IEnumerable<ModifierGroupViewModel> model = _userMenu.GetAllModifierGroup();
        return PartialView("_ModifierTab", model);
    }

    //Add Modifier
     [HttpPost]
    public async Task<IActionResult> AddModifier(ModifierGroupViewModel model , string modifierList)
    {
        if (!string.IsNullOrEmpty(modifierList))
        {
            model.ModifierItemList = JsonSerializer.Deserialize<List<ModifierItemViewModel>>(modifierList);
        }

        var isAdded = await _userMenu.AddModifier(model);
        if (isAdded)
        {
            return RedirectToAction("Index", "Home");
        }
        

        string result = "";
        bool isCreated = true;

        if (result.Equals("true"))
        {
            return Json(new { success = true , message = result});
        }
        else
        {
            return Json(new { success = false, errorMessage = result});
        }

    }
  
    // Edit Modifier

    [HttpPost]
    public async Task<IActionResult> EditModifier(ModifierGroupViewModel model ,  string ExistingmodifierList )
    {

        if (!string.IsNullOrEmpty(ExistingmodifierList))
        {
            model.ModifierItemList = JsonSerializer.Deserialize<List<ModifierItemViewModel>>(ExistingmodifierList);
        }

        await _userMenu.UpdateModifier(model);
        return RedirectToAction("Index", "Home");
    }

    //Delete Modifier

    [HttpPost]

    public async Task<IActionResult> DeleteModifer(int id)
    {
        var existingModifier = await _userMenu.GetModifierByIdForDelete(id);
        return RedirectToAction("ItemsByModifier", "Menu");
    }

   
    public async Task<IActionResult> ItemsByModifier(int id , int pageNo = 1 , int pageSize=3, string searchKey = "" )
    {
        var items = await _userMenu.GetItemsByModifier(id,pageNo,pageSize, searchKey);
        items.Page.Modifierid = id;
        items.Page.searchKey = searchKey;

        return PartialView("_ModifierPartial", items);
    }

    [HttpPost]
    public async Task<IActionResult> AddModifierItem(menuviewmodel model)
    {
        var isAdded = await _userMenu.AddModifierItem(model);
        if (isAdded)
        {
            return RedirectToAction("Index", "Home");
        }
        else
        {
            return Content("error");
        }
    }

    public async Task<IActionResult> EditModifierItem(int id)
    {
        var item =await _userMenu.GetEditModifierItem(id);
        item.ModifierId = id;

        return PartialView("_EditModifierItemPartial",item);                                 
    }

    [HttpPost]
    public async Task<IActionResult> EditModifierItems(AddModifierViewModel model)
    {
        var modifiers = _userMenu.EditModifierItem(model);
        return RedirectToAction("Items","Menu");                                  
    }

    [HttpPost]

    public async Task<IActionResult> DeleteModifierItem(int id , int modifiergroupId)
    {
        var existingmodifier =  _userMenu.GetModifierItemForDeleteById(id,modifiergroupId);
        return RedirectToAction("Items", "Menu");
    }

     [HttpPost]
    public IActionResult DeleteCombineForModifier(List<int> modifierList , int modifiergroupId)
    {
        _userMenu.DeleteModifiersAsync(modifierList , modifiergroupId);
        return Json( new { success = true, message = "hi"});
    }

      public async Task<IActionResult> ItemsByExistingModifier( int pageNo = 1 , int pageSize=3, string searchKey = "" )
    {
        var items = await _userMenu.GetItemsByExistingModifier(pageNo,pageSize, searchKey);
        items.Page.searchKey = searchKey;

        return PartialView("_AddExistingModifier", items);
    }

    public async Task<IActionResult> GetExisistingsModifier(int id)
    {
        var items = await _userMenu.GetExistingModifierItems(id);
        return PartialView("_EditExistingModifier", items);
    }

}
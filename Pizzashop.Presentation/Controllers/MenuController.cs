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
        int selectedCategoryId = id != 0 ? id : categories.First().CategoryId;

        var items = await _userMenu.GetItemsByCategory(selectedCategoryId,  pageNo ,  pageSize);
        items.Categoryid = selectedCategoryId;

        var viewModel = new menuviewmodel
        {
            Categories = categories,
            pagination = items,
            units = units,
            Modifiers = modifiers
        };

        return View(viewModel);

    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(Categoryviewmodel model)
    {
        var isAdded = await _userMenu.AddCategory(model);
        if (isAdded)
        {
            return RedirectToAction("Index", "Home");
        }
        else
        {
            return Content("error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditCategory(Categoryviewmodel model)
    {
        await _userMenu.UpdateCategory(model);
        return RedirectToAction("Index", "Home");
    }


    [HttpPost]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var existingCategory = await _userMenu.GetModifierById(id);

        return RedirectToAction("Index", "Home");

    }

    [Route("Menu/ItemsByCategory")]
    public async Task<IActionResult> ItemsByCategory(int id , int pageNo = 1 , int pageSize=3, string searchKey = "" )
    {
        var items = await _userMenu.GetItemsByCategory(id,pageNo,pageSize, searchKey);
        items.Categoryid = id;
        items.searchKey = searchKey;

        return PartialView("_ItemsPartial", items);
    }


    [HttpPost]
    public async Task<IActionResult> Items(menuviewmodel model)
    {
        var isAdded = await _userMenu.AddNewItem(model);

        return RedirectToAction("Index","Home");
    }

     public async Task<IActionResult> EditItem(int id)
    {
        var item =await _userMenu.GetEditItem(id);
        item.Itemid = id;

        return PartialView("_EditItemPartial",item);                                 
    }

    [HttpPost]
    public async Task<IActionResult> EditItems(EditItemviewmodel model)
    {
        var item = _userMenu.EditItem(model);
        return RedirectToAction("Items");                                  
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
    public async Task<IActionResult> AddModifier(ModifierGroupViewModel model)
    {
        var isAdded = await _userMenu.AddModifier(model);
        if (isAdded)
        {
            return RedirectToAction("Index", "Home");
        }
        else
        {
            return Content("error");
        }
    }
  
    // Edit Modifier

    [HttpPost]
    public async Task<IActionResult> EditModifier(ModifierGroupViewModel model)
    {
        await _userMenu.UpdateModifier(model);
        return RedirectToAction("Index", "Home");
    }

    //Delete Modifier

    [HttpPost]

    public async Task<IActionResult> DeleteModifer(int id)
    {
        var existingModifier = await _userMenu.GetModifierByIdForDelete(id);
        return RedirectToAction("Index", "Home");
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
    public async Task<IActionResult> EditModifierItems(EditModifierViewModel model)
    {
        var modifiers = _userMenu.EditModifierItem(model);
        return RedirectToAction("Items","Menu");                                  
    }

    [HttpPost]

    public async Task<IActionResult> DeleteModifierItem(int id)
    {
        var existingmodifier =  _userMenu.GetModifierItemForDeleteById(id);
        return RedirectToAction("Items", "Menu");
    }

     [HttpPost]
    public IActionResult DeleteCombineForModifier(List<int> modifierList)
    {
        _userMenu.DeleteModifiersAsync(modifierList);
        return Json( new { success = true, message = "hi"});
    }
}
using BAL.Models.Interfaces;
using iText.Commons.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Pizzashop.Presentation.Controllers;

public class OrderAppMenu : Controller
{

    private readonly IOrderAppMenu _orderAppMenu;

    public OrderAppMenu(IOrderAppMenu orderAppMenu)
    {
         _orderAppMenu = orderAppMenu;
    }

    public async Task<IActionResult> OrderMenu()
    {
        return View();
    }

    public async Task<IActionResult> GetCategories()
    {
        var categories = await _orderAppMenu.GetCategories();
        return PartialView("_MenuCategories", categories);
    }

    public async Task<IActionResult> GetItems(int CategoryId , string SearchKey)
    {
        var items = await _orderAppMenu.GetItems(CategoryId,SearchKey);
        return PartialView("_MenuItems", items);
    }

    public async Task<IActionResult> GetFavouriteItems(string SearchKey)
    {
        var items = await _orderAppMenu.GetFavouriteItems(SearchKey);
        return PartialView("_MenuItems", items);
    }
   
   public async Task<IActionResult> UpdateFavourite (int ItemId)
    {
        var result = await _orderAppMenu.UpdateFavourite(ItemId);
        if (result)
        {
            return Json(new { success = true });
        }
        else
        {
            return Json(new { success = false });
        }
    }

    public async Task<IActionResult> GetModalData(int ItemId)
    {
        var item = await _orderAppMenu.GetModalData(ItemId);

        if(item.ModifierGroupList.Count !=0)
        {
        return PartialView("_MenuItemModal", item);
        }
        return Json(new { success = false });
    }
    
    

}

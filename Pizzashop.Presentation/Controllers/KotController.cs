using BAL.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Pizzashop.DAL.ViewModels;
// using static Pizzashop.DAL.ViewModels.ItemUpdateviewmodel;
using static Pizzashop.DAL.ViewModels.Kotviewmodel;

namespace Pizzashop.Presentation.Controllers;

public class KotController : Controller
{

    private readonly IKotService _kotService;

    public KotController(IKotService kotService)
    {
        _kotService = kotService;
    }

    public async Task<IActionResult> Kot()
    {
        var categories = await _kotService.GetCategories();
        return View(categories);
    }
    public async Task<IActionResult> KotData(string status = "In Progress", int categoryId = 0)
    {
        var kotData = await _kotService.GetKotDataAsync(status, categoryId);
        ViewBag.ActivateCategoryId = categoryId;
        return  PartialView("_kotcard",kotData);
    }

    public async Task<IActionResult> ChangeQuantityModal( int id , string status) 
    {
        var kotDetails = await _kotService.GetKotDetailsAsync(id,status);
        ViewBag.OrderStatus = kotDetails.OrderStatus;
        return PartialView("_ChangeQuantityModal", kotDetails);
    }

    [HttpPost]
    public async Task<IActionResult> updateQuantity(int orderId,string status,List<ItemUpdateModel> updates)
    {
        var results = new List<bool>();

        foreach (var update in updates)
        {
            if(update.Quantity>0)
            {
            var result = await _kotService.UpdateQuantityAsync(orderId,status, update.ItemId, update.Quantity);
            results.Add(result);      
            }
        }

        bool success = results.All(r => r);

        if (success)
        {
            return Json(new { success = true });
        }
        else
        {
            return Json(new { success = false });
        }
    }

}
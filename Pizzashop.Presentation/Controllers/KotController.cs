using AspNetCoreHero.ToastNotification.Abstractions;
using BAL.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Pizzashop.DAL.ViewModels;


namespace Pizzashop.Presentation.Controllers;

public class KotController : Controller
{

    private readonly IKotService _kotService;
    private readonly IHubContext<NotificationHub> _hubcontext;
    private readonly INotyfService _notyf;

    public KotController(IKotService kotService , IHubContext<NotificationHub> hubcontext , INotyfService notyf)
    {
        _hubcontext = hubcontext;
        _kotService = kotService;
        _notyf = notyf;
    }

    public async Task<IActionResult> Kot()
    {
        // var categories = await _kotService.GetCategories();
        var categories = await _kotService.GetCategoriesFromFunctionAsync();
        //  _notyf.Success("Item updated successfully");
        return View(categories);
    }
    public async Task<IActionResult> KotData(string status = "In Progress", int categoryId = 0)
    {
        var kotDataes = await _kotService.GetKotData(status, categoryId);
        // var kotData = await _kotService.GetKotDataAsync(status, categoryId);
        ViewBag.ActivateCategoryId = categoryId;
        return PartialView("_kotcard",kotDataes);
    }

    public async Task<IActionResult> ChangeQuantityModal( int id , string status , int CategoryId) 
    {
        var kotdata = await _kotService.GetKotCardData(id,status,CategoryId);
        // var kotDetails = await _kotService.GetKotDetailsAsync(id,status);
        ViewBag.OrderStatus = kotdata.OrderStatus;
        return PartialView("_ChangeQuantityModal", kotdata);
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
         _notyf.Success("Item updated successfully");
        bool success = results.All(r => r);

        if (success)
        {
            await _hubcontext.Clients.All.SendAsync("KotMessage", "A kot was updated succesfully.");
            return Json(new { success = true });
        }
        else
        {
            return Json(new { success = false });
        }
    }

}
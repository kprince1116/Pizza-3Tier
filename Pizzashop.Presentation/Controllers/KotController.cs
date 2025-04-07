using BAL.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Pizzashop.DAL.ViewModels;
using static Pizzashop.DAL.ViewModels.Kotviewmodel;

namespace Pizzashop.Presentation.Controllers;

public class KotController : Controller
{

    private readonly IKotService _kotService;

    public KotController(IKotService kotService)
    {
        _kotService = kotService;
    }
    public async Task<IActionResult> Kot(string status="In Progress",int categoryId=0)
    {
        var kotData = await _kotService.GetKotDataAsync(status,categoryId);

        return View(kotData);
    }

    public async Task<IActionResult> ChangeQuantityModal(int id)
    {
        var kotDetails = await _kotService.GetKotDetailsAsync(id);
        ViewBag.OrderStatus = kotDetails.OrderStatus;
        return PartialView("_ChangeQuantityModal", kotDetails);
    }

    [HttpPost]

    public async Task<IActionResult> updateQuantity(int orderId,int itemId, int quantity)
    {
        var result = await _kotService.UpdateQuantityAsync(orderId,itemId, quantity);
        if (result)
        {
            return Json(new { success = true });
        }
        else
        {
            return Json(new { success = false });
        }
    }

}





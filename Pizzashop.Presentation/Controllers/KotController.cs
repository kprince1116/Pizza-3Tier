using BAL.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Pizzashop.DAL.ViewModels;

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

        var orders = await _kotService.GetOrders();

          return View(categories);
       }
}

   



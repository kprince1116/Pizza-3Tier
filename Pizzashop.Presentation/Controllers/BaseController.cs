using BAL.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pizzashop.DAL.ViewModels;

namespace Pizzashop.Presentation.Controllers;

public class BaseController : Controller
{
    private readonly IPermissions _permissionService;

    public BaseController(IPermissions permissionService)
    {
        _permissionService = permissionService;
    }

    public override async void OnActionExecuting(ActionExecutingContext context)
{  
    try
    {
        ViewBag.CanViewOrders = await _permissionService.HasPermission("Order", ActionPermissions.CanView);
        Console.WriteLine("VVV: " +  ViewBag.CanViewOrders);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

    
}
}
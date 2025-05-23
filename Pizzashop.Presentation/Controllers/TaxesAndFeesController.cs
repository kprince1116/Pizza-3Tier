using BAL.Attributes;
using BAL.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Pizzashop.DAL.ViewModels;

namespace Pizzashop.Presentation.Controllers;

public class TaxesAndFeesController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ITaxesAndFessService _taxesAndFeesService;
    private readonly IHubContext<NotificationHub> _hubcontext;

    public TaxesAndFeesController(IConfiguration configuration, ITaxesAndFessService taxesAndFeesService, IHubContext<NotificationHub> hubcontext)
    {
        _configuration = configuration;
        _taxesAndFeesService = taxesAndFeesService;
        _hubcontext = hubcontext;
    }

    [Authorize(Roles = "Account_Manager,Admin")]
    [_AuthPermissionAttribute("TaxAndFee", ActionPermissions.CanView)]
    public IActionResult TaxesAndFees()
    {
        return View();
    }

    [_AuthPermissionAttribute("TaxAndFee", ActionPermissions.CanView)]
    public async Task<IActionResult> GetTaxList(int pageNo = 1, int pageSize = 7, string searchKey = "")
    {
        var taxlist = await _taxesAndFeesService.GetTaxDeails(pageNo, pageSize, searchKey);
        return PartialView("_TaxPartial", taxlist);
    }

    [_AuthPermissionAttribute("TaxAndFee", ActionPermissions.CanAddEdit)]
    [HttpPost]
    public async Task<IActionResult> AddTax(Taxviewmodel model)
    {
        var isAdded = await _taxesAndFeesService.AddTax(model);

        if (isAdded)
        {
            await _hubcontext.Clients.All.SendAsync("TaxMessage", "A tax was deleted.");
            TempData["AddTaxSuccess"] = true;
            return RedirectToAction("TaxesAndFees", "TaxesAndFees");
        }
        else
        {
            TempData["AddTaxError"] = "A Tax with the same name already exists.";
            return RedirectToAction("TaxesAndFees", "TaxesAndFees");
        }

    }

    [_AuthPermissionAttribute("TaxAndFee", ActionPermissions.CanDelete)]
    [HttpPost]
    public async Task<IActionResult> DeleteTax(int id)
    {
        var isdelete = await _taxesAndFeesService.DeleteTax(id);
        if (isdelete)
        {
            await _hubcontext.Clients.All.SendAsync("TaxMessage", "A tax was deleted.");
            TempData["DeleteTaxSuccess"] = true;
            return RedirectToAction("TaxesAndFees", "TaxesAndFees");
        }
        else
        {
            return Content("error");
        }
    }

    [_AuthPermissionAttribute("TaxAndFee", ActionPermissions.CanAddEdit)]
    public async Task<IActionResult> EditTax(int id)
    {
        var item = await _taxesAndFeesService.GetEditTax(id);

        return PartialView("_EditTaxPartial", item);
    }

    [_AuthPermissionAttribute("TaxAndFee", ActionPermissions.CanAddEdit)]
    [HttpPost]
    public async Task<IActionResult> EditTax(EditTaxviewmodel model)
    {
        var isEdit = await _taxesAndFeesService.EditTax(model);
        if (isEdit)
        {
            await _hubcontext.Clients.All.SendAsync("TaxMessage", "A tax was deleted.");
            TempData["EditTaxSuccess"] = true;
            return RedirectToAction("TaxesAndFees", "TaxesAndFees");
        }
        else
        {
            TempData["EditTaxError"] = "A Tax with the same name already exists.";
            return RedirectToAction("TaxesAndFees", "TaxesAndFees");
        }
    }

    [_AuthPermissionAttribute("TaxAndFee", ActionPermissions.CanAddEdit)]
    [HttpPost]
    public async Task<IActionResult> EditTaxAvailabity(int id, bool isAvailable)
    {
        var item = await _taxesAndFeesService.EditTaxAvailabity(id, isAvailable);
        if (item)
        {
            await _hubcontext.Clients.All.SendAsync("TaxMessage", "A tax was deleted.");
            return Json(new { success = true, message = "hi" });
        }
        else
        {
            return Json(new { success = false, message = "hi" });
        }
    }

    [_AuthPermissionAttribute("TaxAndFee", ActionPermissions.CanAddEdit)]
    [HttpPost]
    public async Task<IActionResult> EditTaxDefault(int id, bool isAvailable)
    {
        var item = await _taxesAndFeesService.EditTaxDefault(id, isAvailable);
        if (item)
        {
            await _hubcontext.Clients.All.SendAsync("TaxMessage", "A tax was deleted.");
            return Json(new { success = true, message = "hi" });
        }
        else
        {
            return Json(new { success = false, message = "hi" });
        }
    }
}

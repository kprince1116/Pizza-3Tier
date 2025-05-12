using BAL.Models.Interfaces;
using iText.Commons.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Pizzashop.DAL.ViewModels;

namespace Pizzashop.Presentation.Controllers;

public class WaitingListController : Controller
{
    private readonly IWaitingService _waitingService;  
    private readonly IKotTableService _kotTableService; 
    private readonly IHubContext<NotificationHub> _hubcontext;

    public WaitingListController(IWaitingService waitingService,IKotTableService kotTableService , IHubContext<NotificationHub> hubcontext)
    {
        _hubcontext = hubcontext;
        _kotTableService = kotTableService;
        _waitingService = waitingService;
    }

    [Authorize(Roles = "Account_Manager,Admin")]
    public async Task<IActionResult> WaitingList()
    {
        var section = await _waitingService.GetSections();
        return View(section);
    }

     public async Task<IActionResult> openWaitingTokenModal()
    {
        waitingtokenviewmodel obj = new waitingtokenviewmodel();
        var sections = await _kotTableService.GetSectionList();
        obj.sections =sections;
        return PartialView("_AddWaitingPartialView",obj);
    }
    public async Task<IActionResult> GetWaitingList(int sectionId)
    {
        var waitingList = await _waitingService.GetWaitingList(sectionId);
        return PartialView("_WaitingListPartialView", waitingList);
    }

    public async Task<IActionResult> GetCustomerDetailByEmail(int sectionid,string Email)
    {
        var customerDetails = await _waitingService.GetCustomerDetailsByEmail(sectionid,Email);
        return PartialView("_AddWaitingPartialview", customerDetails);
    }

    [HttpPost]
    public async Task<IActionResult> AddWaitingToken(waitingtokenviewmodel model)
    {
        var result = await _kotTableService.AddWaitingToken(model);
        if(result){
        await _hubcontext.Clients.All.SendAsync("WaitingMessage", "A tax was deleted.");
         return Json(new { success = true });
        }
        else{
            return Json(new { success = false });
        }
    }

    public async Task<IActionResult> EditToken(int id)
    {
        var result = await _waitingService.EditToken(id);
        return PartialView("_EditWaitingToken", result);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateToken(waitingtokenviewmodel model)
    {
        var result = await _waitingService.UpdateWaitingToken(model);

        if(result){
        await _hubcontext.Clients.All.SendAsync("WaitingMessage", "A tax was deleted.");
        TempData["EditWaitingSuccess"] = true;
         return RedirectToAction("WaitingList", "WaitingList");
        }
        else{
            return Content("error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteToken(int Id)
    {
        var result = await _waitingService.DeleteToken(Id);
         if(result){
         await _hubcontext.Clients.All.SendAsync("WaitingMessage", "A tax was deleted.");
         TempData["DeleteWaitingSuccess"] = true;
         return RedirectToAction("WaitingList", "WaitingList");
        }
        else{
            return Content("error");
        }
    }

    public async Task<IActionResult> GetTableDeatails()
    {
        var tableDetails = await _waitingService.GetTableDetails();
        return PartialView("_AssignTable", tableDetails);
    }

    [HttpPost]

    public async Task<IActionResult> AssignTable(waitingtokenviewmodel model)
    {
        var result = await _waitingService.AssignTable(model);

        var orderId = result;

        var redirectUrl =  "/OrderAppMenu/OrderMenu?orderId=" + orderId ;
        
        return Json(new { success = true , url = redirectUrl });
        
    }

}
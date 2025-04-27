using BAL.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Pizzashop.Presentation.Controllers;

using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Pizzashop.DAL.ViewModels;


public class KotTableController : Controller
{
    private readonly IKotTableService _kotTableService;

    public KotTableController(IKotTableService kotTableService)
    {
        _kotTableService = kotTableService;
    }

    public async Task<IActionResult> KotTable()
    {
       var sections = await _kotTableService.GetSections();
       return View(sections);
    }

    public async Task<IActionResult> openWaitingTokenModal()
    {
        waitingtokenviewmodel obj = new waitingtokenviewmodel();
        var sections = await _kotTableService.GetSectionList();
        obj.sections =sections;
        return PartialView("_AddWaitingToken",obj);
    }

    [HttpPost]
    public async Task<IActionResult> AddWaitingToken(waitingtokenviewmodel model)
    {
        var result = await _kotTableService.AddWaitingToken(model);
        if(result){
         return Json(new { success = true });
        }
        else{
            return Json(new { success = false });
        }
    }
    
    public async Task<IActionResult> GetWaitingList(int id)
    {
        var waitingList = await _kotTableService.GetWaitingList(id);
        return PartialView("_WaitingList", waitingList);
    }

    public async Task<IActionResult> GetCustomerDetail(int id)
    {
        var customerDetails = await _kotTableService.GetCustomerDetails(id);
        return PartialView("_CustomerDetails", customerDetails);
    }
    public async Task<IActionResult> GetCustomerDetailByEmail(int sectionid , string Email)
    {
        var customerDetails = await _kotTableService.GetCustomerDetailsByEmail(sectionid,Email);
        return PartialView("_CustomerDetails", customerDetails);
    }
  
    [HttpPost]

        public async Task<IActionResult> AssignTable(waitingtokenviewmodel model, string SelectedTables)
{
    try
    {
        var tableIds = JsonSerializer.Deserialize<List<int>>(SelectedTables);

        var result = await _kotTableService.AssignTable(model,tableIds);
        var customerId = result.Item1;
        var orderId = result.Item2;
       
        var redirectUrl =  "/OrderAppMenu/OrderMenu?customerId=" + customerId + "&orderId=" + orderId;
        return Json(new { success = true , url = redirectUrl });
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
         return Json(new { success = false });
    }
}

    
}


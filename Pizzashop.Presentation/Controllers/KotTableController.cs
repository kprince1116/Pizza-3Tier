using BAL.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Pizzashop.Presentation.Controllers;

using System.Formats.Asn1;
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
    
    public async Task<IActionResult> GetWaitingList()
    {
        var waitingList = await _kotTableService.GetWaitingList();
        return PartialView("_WaitingList", waitingList);
    }

 
}


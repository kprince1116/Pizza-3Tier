using BAL.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Pizzashop.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc; 

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

}


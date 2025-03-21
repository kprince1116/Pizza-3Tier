using BAL.Models.Interfaces;
using BAL.Services;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Pizzashop.DAL.ViewModels;

namespace Pizzashop.Presentation.Controllers;

public class TableController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ITable _table;

    public TableController(IConfiguration configuration,  ITable table)
    {
        _configuration = configuration;
        _table = table;
    }

    public async Task<IActionResult> Table(int id, int pageNo = 1 , int pageSize=3 , string searchKey = "" )
    {
        var sections = _table.GetSections();
        int selectedSectionId = id != 0 ? id : sections.First().Sectionid;

        var tables = await _table.GetTablesBySection(selectedSectionId , pageNo , pageSize , searchKey);

        var viewModel = new Tablesviewmodel
        {
            Sections = sections,
            Tables = tables
        };

        return View(viewModel);
    }

    //CRUD SECTIONS
    [HttpPost]
    public async Task<IActionResult> AddSection(Sectionviemodel model)
    {
        var isAdded = await _table.AddSection(model);
        if (isAdded)
        {
            return RedirectToAction("Table", "Table");
        }
        else
        {
            return Content("error");
        }
    }

    [HttpPost]

    public async Task<IActionResult> EditSection(Sectionviemodel model)
    {
        await _table.EditSection(model);
    
        return RedirectToAction("Table", "Table");
        
    }

    [HttpPost]

    public async Task<IActionResult> DeleteSection(int id)
    {
        var existingSection = await _table.GetSectionByIdForDelte(id);
        return  RedirectToAction("Table", "Table");
    }

    public async Task<IActionResult> TablesBySection(int id , int pageNo = 1 , int pageSize=3, string searchKey = "" )
    {
        var tables = await _table.GetTablesBySection(id,pageNo,pageSize, searchKey);

        return PartialView("_TablePartial", tables);
    }

    [HttpPost]

    public async Task<IActionResult> AddTable(Tablesviewmodel model)
    {
         await _table.AddTable(model);
        return  RedirectToAction("Table", "Table");
    }

     public async Task<IActionResult> EditTable(int id)
    {
        var item =await _table.GetEditTable(id);
        item.Tableid = id;

        return PartialView("_EditTablePartial",item);                                 
    }

    [HttpPost]
    public async Task<IActionResult> EditTable(EditTableviewmodel model)
    {
         await _table.EditTable(model);
        return  RedirectToAction("Table", "Table");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTable(int id)
    {
        var existingSection = await _table.GetTableByIdForDelte(id);
        return  RedirectToAction("Table", "Table");
    }

    [HttpPost]

    public async Task<IActionResult> DeleteCombine(List<int> tableLists)
    {
        _table.DeleteTableAsync(tableLists);
        return Json( new { success = true, message = "hi"});
    }

}

using BAL.Attributes;
using BAL.Models.Interfaces;
using BAL.Services;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Pizzashop.DAL.ViewModels;

namespace Pizzashop.Presentation.Controllers;

public class TableController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ITable _table;
    private readonly IHubContext<NotificationHub> _hubcontext;

    public TableController(IConfiguration configuration, ITable table,IHubContext<NotificationHub> hubcontext)
    {
         _hubcontext = hubcontext;
        _configuration = configuration;
        _table = table;
    }

    [Authorize(Roles = "Account_Manager,Admin")]
    [_AuthPermissionAttribute("TableAndSection", ActionPermissions.CanView)]
    public async Task<IActionResult> Table(int id, int pageNo = 1, int pageSize = 3, string searchKey = "")
    {
        var sections = _table.GetSections();
        int selectedSectionId = id != 0 ? id : sections.First().Sectionid;

        var tables = await _table.GetTablesBySection(selectedSectionId, pageNo, pageSize, searchKey);

        var viewModel = new Tablesviewmodel
        {
            Sections = sections,
            Tables = tables
        };

        return View(viewModel);
    }

    [_AuthPermissionAttribute("TableAndSection", ActionPermissions.CanAddEdit)]
    //CRUD SECTIONS
    [HttpPost]
    public async Task<IActionResult> AddSection(Tablesviewmodel model)
    {
        var isAdded = await _table.AddSection(model);
        if (isAdded)
        {
            await _hubcontext.Clients.All.SendAsync("TableMessage", "A section was added.");
            return Json(new { success = true });
        }
        else
        {
            return Json(new { success = false });
        }

    }

    [_AuthPermissionAttribute("TableAndSection", ActionPermissions.CanAddEdit)]
    [HttpPost]
    public async Task<IActionResult> EditSection(Tablesviewmodel model)
    {
        var isEdit = await _table.EditSection(model);
        if (isEdit)
        {
            await _hubcontext.Clients.All.SendAsync("TableMessage", "A section was added.");
            return Json(new { success = true });
        }
        else
        {
            return Json(new { success = false });
        }

    }

    [_AuthPermissionAttribute("TableAndSection", ActionPermissions.CanDelete)]
    [HttpPost]

    public async Task<IActionResult> DeleteSection(int id)
    {
        var existingSection = await _table.GetSectionByIdForDelte(id);
        if (existingSection)
        {
            await _hubcontext.Clients.All.SendAsync("TableMessage", "A section was added.");
            TempData["DeleteSectionSuccess"] = true;
            return RedirectToAction("Table", "Table");
        }
        else
        {
            return Content("error");
        }
    }

    public async Task<IActionResult> TablesBySection(int id, int pageNo = 1, int pageSize = 3, string searchKey = "")
    {
        var tables = await _table.GetTablesBySection(id, pageNo, pageSize, searchKey);

        return PartialView("_TablePartial", tables);
    }

    [_AuthPermissionAttribute("TableAndSection", ActionPermissions.CanAddEdit)]
    [HttpPost]
    public async Task<IActionResult> AddTable(Tablesviewmodel model)
    {
        var isAdded = await _table.AddTable(model);
        if (isAdded)
        {
            await _hubcontext.Clients.All.SendAsync("TableMessage", "A section was added.");
            TempData["AddTableSuccess"] = true;
            return RedirectToAction("Table", "Table");
        }
        else
        {
            return Content("error");
        }
    }


    public async Task<IActionResult> EditTable(int id)
    {
        var item = await _table.GetEditTable(id);
        item.Tableid = id;

        return PartialView("_EditTablePartial", item);

    }

    [_AuthPermissionAttribute("TableAndSection", ActionPermissions.CanAddEdit)]
    [HttpPost]
    public async Task<IActionResult> EditTable(EditTableviewmodel model)
    {

        var isEdit = await _table.EditTable(model);
        if (isEdit)
        {
            await _hubcontext.Clients.All.SendAsync("TableMessage", "A section was added.");
            return Json(new { success = true });
        }
        else
        {
            return Json(new { success = false, message = "Table update failed." });
        }
    }


    [_AuthPermissionAttribute("TableAndSection", ActionPermissions.CanDelete)]
    [HttpPost]
    public async Task<IActionResult> DeleteTable(int id)
    {
        var isDelete = await _table.GetTableByIdForDelte(id);
        if (isDelete)
        {
            await _hubcontext.Clients.All.SendAsync("TableMessage", "A section was added.");
            TempData["DeleteTableSuccess"] = true;
            return RedirectToAction("Table", "Table");
        }
        else
        {
            return Content("error");
        }
    }

    [_AuthPermissionAttribute("TableAndSection", ActionPermissions.CanDelete)]
    [HttpPost]
    public async Task<IActionResult> DeleteCombine(List<int> tableLists)
    {
        var isDelete =  _table.DeleteTableAsync(tableLists);
        if(isDelete !=null)
        {
            await _hubcontext.Clients.All.SendAsync("TableMessage", "A section was added.");
            return Json(new { success = true, message = "hi" });
        }
        else
        {
             return Json(new { success = false, message = "hi" });
        }
    }

}

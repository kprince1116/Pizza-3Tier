using Microsoft.AspNetCore.Mvc;

namespace Pizzashop.Presentation.Controllers;

public class WaitingListController : Controller
{
    public IActionResult WaitingList()
    {
        return View();
    }
}

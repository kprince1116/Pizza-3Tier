using Microsoft.AspNetCore.Mvc;

namespace Pizzashop.Presentation.Controllers;

public class AuthController : Controller
{
     [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    // [HttpPost]

    // public IActionResult Auth()
    // {
    //     return RedirectToAction("Index","Home");
    // }
}

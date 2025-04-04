using Microsoft.AspNetCore.Mvc;

namespace Pizzashop.Presentation.Controllers;

public class OrderAppController : Controller
{
    private readonly IConfiguration _configuration;

    public OrderAppController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult OrderApp()
    {
        return View();
    }

}

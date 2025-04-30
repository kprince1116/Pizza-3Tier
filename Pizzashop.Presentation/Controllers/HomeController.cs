using System.Diagnostics;
using BAL.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pizzashop.Presentation.Models;

namespace Pizzashop.Presentation.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly IDashboard _Dashboard;

    public HomeController(ILogger<HomeController> logger , IDashboard Dashboard)
    {
        _logger = logger;
        _Dashboard =  Dashboard;
    }

    [Authorize(Roles = "Account_Manager,Admin")]
    public async  Task<IActionResult> Index()
    {
       return View();
    }

    public async  Task<IActionResult> DashboardData(string time , string fromdate , string todate )
    {   
        var data =await _Dashboard.GetDashboardData(time,fromdate,todate);
        return PartialView("_dashboardData",data);
    }
        

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

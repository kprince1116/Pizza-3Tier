using BAL.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Pizzashop.Presentation.Controllers;

public class OrderController : Controller
{
     private readonly IConfiguration _configuration;

     private readonly IOrderservice _orderservice;

      public OrderController(IConfiguration configuration,IOrderservice orderservice)
    {
        _configuration = configuration; 
        _orderservice = orderservice;
    }

    public IActionResult Order()
    {
        return View();
    }

       public async Task<IActionResult> GetOrderList(int pageNo = 1, int pageSize = 3, string searchKey = "" , string sortby = "" , string sortdirection = "" , string statusFilter="" , string timeFilter = "",string  fromDate = "" , string toDate="")
    {
        var taxlist = await _orderservice.GetOrderDetails( pageNo , pageSize ,  searchKey , sortby , sortdirection ,statusFilter , timeFilter , fromDate , toDate);
        return PartialView("_orderPartial",taxlist);
    }


}

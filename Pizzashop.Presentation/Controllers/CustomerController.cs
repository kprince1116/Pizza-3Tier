using BAL.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Pizzashop.Presentation.Controllers;

public class CustomerController : Controller
{
    private readonly IConfiguration _configuration;

     private readonly ICustomerService _customerservice;

      public CustomerController(IConfiguration configuration,ICustomerService customerservice)
    {
        _configuration = configuration; 
        _customerservice = customerservice;
    }

    public IActionResult Customer()
    {
        return View();
    }

    public async Task<IActionResult> GetCustomerDetails(int pageNo = 1, int pageSize = 3, string searchKey = "")
    {
        var customer = await _customerservice.GetCustomerDetails(pageNo,pageSize,searchKey);
         return PartialView("_customerPartial",customer);
    }

}

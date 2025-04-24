using System.Text.Json;
using BAL.Models.Interfaces;
using iText.Commons.Utils;
using Microsoft.AspNetCore.Mvc;
using Pizzashop.DAL.ViewModels;

namespace Pizzashop.Presentation.Controllers;

public class OrderAppMenu : Controller
{

    private readonly IOrderAppMenu _orderAppMenu;

    public OrderAppMenu(IOrderAppMenu orderAppMenu)
    {
         _orderAppMenu = orderAppMenu;
    }

    public async Task<IActionResult> OrderMenu(int customerId , int orderId)
    {
        if(orderId == 0 )
        {
            OrderAppMenuviewmodel model = new OrderAppMenuviewmodel{
            CustomerId = customerId,
            OrderId = orderId   
        };
        return View(model);
        }
        else
        {
        var orderdata = await _orderAppMenu.GetOrderData(orderId);
        OrderAppMenuviewmodel model = new OrderAppMenuviewmodel{
            CustomerId = customerId,
            OrderId = orderId,
            orderitems = orderdata.orderitems,
            orderTax = orderdata.orderTax
        };
        return View(model);
        }
        
    }

    public async Task<IActionResult> GetCategories()
    {
        var categories = await _orderAppMenu.GetCategories();
        return PartialView("_MenuCategories", categories);
    }

    public async Task<IActionResult> GetItems(int CategoryId , string SearchKey)
    {
        var items = await _orderAppMenu.GetItems(CategoryId,SearchKey);
        return PartialView("_MenuItems", items);
    }

    public async Task<IActionResult> GetFavouriteItems(string SearchKey)
    {
        var items = await _orderAppMenu.GetFavouriteItems(SearchKey);
        return PartialView("_MenuItems", items);
    }
   
   public async Task<IActionResult> UpdateFavourite (int ItemId)
    {
        var result = await _orderAppMenu.UpdateFavourite(ItemId);
        if (result)
        {
            return Json(new { success = true });
        }
        else
        {
            return Json(new { success = false });
        }
    }

    public async Task<IActionResult> GetModalData(int ItemId)
    {
        var item = await _orderAppMenu.GetModalData(ItemId);

        if(item.ModifierGroupList.Count !=0)
        {
        return PartialView("_MenuItemModal", item);
        }
        return Json(new { success = false });
    }
    
    public async Task<IActionResult> GetOrderData(int OrderId)
    {
        var orderData = await _orderAppMenu.GetOrderData(OrderId);
        return PartialView("_OrderData", orderData);
    }

    public async Task<IActionResult> GetCustomerDetails(int OrderId)
    {
        var customerDetails = await _orderAppMenu.GetCustomerDetails(OrderId);
        return PartialView("_CustomerDetails", customerDetails);
    }

    [HttpPost]
    public async Task<IActionResult> SaveCustomerDetails(CustomerDetailsForOrderViewModel model)
    {
        var orderDetails = await _orderAppMenu.SaveCustomerDetails(model);
        if(orderDetails == null)
        {
            return Json(new { success = false });
        }
        return Json(new { success = true });
    }
    
    public async Task<IActionResult> GetOrderComments(int OrderId)
    {
        var orderComments = await _orderAppMenu.GetOrderComments(OrderId);
        return PartialView("_orderwisecomment", orderComments);
    }

    public async Task<IActionResult> PostComment(OrderWiseCommentViewModel model)
    {
        var comment = await _orderAppMenu.PostComment(model);
        if(comment == null)
        {
            return Json(new { success = false });
        }
        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> AppendOrderItems(string OrderItem)
    {
        OrderAppMenuviewmodel model = new OrderAppMenuviewmodel();
        if(!string.IsNullOrEmpty(OrderItem))
        {
            try
            {
                 model.orderitems = JsonSerializer.Deserialize<List<OrderItemviewmodel>>(OrderItem);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
           
        }
        return PartialView("_accordian", model);
    }

}

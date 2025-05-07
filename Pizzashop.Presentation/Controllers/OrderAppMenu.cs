using System.Text.Json;
using BAL.Models.Interfaces;
using iText.Commons.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pizzashop.DAL.ViewModels;

namespace Pizzashop.Presentation.Controllers;

public class OrderAppMenu : Controller
{

    private readonly IOrderAppMenu _orderAppMenu;

    public OrderAppMenu(IOrderAppMenu orderAppMenu)
    {
         _orderAppMenu = orderAppMenu;
    }

     [Authorize(Roles = "Account_Manager,Admin")]

    public async Task<IActionResult> OrderMenu(int customerId , int orderId)
    {
        if(orderId == 0 )
        {
            OrderAppMenuviewmodel model = new OrderAppMenuviewmodel{
            CustomerId = customerId,
            OrderId = orderId,
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
            orderTax = orderdata.orderTax,
            PaymentStatus = orderdata.PaymentStatus
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
    public async Task<IActionResult> GetItemComments(int OrderId)
    {
        var itemComments = await _orderAppMenu.GetItemComments(OrderId);
        return PartialView("_itemwisecomment", itemComments);
    }

    [HttpPost]
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
    public async Task<IActionResult> PostItemComment(Itemwisecommentviewmodel model)
    {
        var comment = await _orderAppMenu.PostItemComment(model);
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
                 model.orderitems = JsonConvert.DeserializeObject<List<OrderItemviewmodel>>(OrderItem);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
           
        }
        return PartialView("_accordian", model);
    }

    [HttpPost]

    public async Task<IActionResult> SaveOrder(string order_id,string order_status,string selected_items,string delete_item,string tax_data,string payment_type)
    {
        int OrderId = JsonConvert.DeserializeObject<int>(order_id);
        string OrderStatus = JsonConvert.DeserializeObject<string>(order_status);
        List<OrderItemviewmodel> save_items = JsonConvert.DeserializeObject<List<OrderItemviewmodel>>(selected_items);
        List<int> delete_items = JsonConvert.DeserializeObject<List<int>>(delete_item);
        List<MenuTaxviewmodel> save_tax = JsonConvert.DeserializeObject<List<MenuTaxviewmodel>>(tax_data);
        string paymenType = JsonConvert.DeserializeObject<string>(payment_type);

        var saveorder = await _orderAppMenu.SaveOrder(OrderId,OrderStatus,save_items,delete_items,save_tax,paymenType);
        return Json(new { success = true });
    }

    public async Task<IActionResult> CheckReadyQuantity(int orderId)
    {
        var check = await _orderAppMenu.CheckReadyQuantity(orderId);

        if(check){
            return Json(new{success = true});
        }
        else{
            return Json (new{success = false});
        }
    }
    public async Task<IActionResult> CheckReadyQuantityForCancel(int orderId)
    {
        var check = await _orderAppMenu.CheckReadyQuantityForCancel(orderId);

        if(check){
            return Json(new{success = true});
        }
        else{
            return Json (new{success = false});
        }
    }

    [HttpPost]
    public async Task<IActionResult> CompleteOrder(int orderId)
    {
        var result = await _orderAppMenu.CompleteOrder(orderId);
        
        if(result)
        {
           return Json(new { success = true });
        }
        else{
            return Json(new { success = false });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CancelOrder(int orderId)
    {
        var result = await _orderAppMenu.CancelOrder(orderId);
        
        if(result)
        {
           return Json(new { success = true });
        }
        else{
            return Json(new { success = false });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddCustomerRatting(OrderAppMenuviewmodel model)
    {
        var result = await _orderAppMenu.AddCustomerRatting(model);
        if(result){
            TempData["SuccessCustomerRating"] = "Customer Rating Successfully added";
             return RedirectToAction("OrderMenu","OrderAppMenu");
        }
        else{
            TempData["FailedCustomerRating"] = "customer rating not added";
            return RedirectToAction("OrderMenu","OrderAppMenu");
        }
    }

}

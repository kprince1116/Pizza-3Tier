using BAL.Models.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pizzashop.DAL.ViewModels;

namespace BAL.Services;

public class Orderservice : IOrderservice
{
    private readonly IOrderRepository _orderRepository;

    public Orderservice (IOrderRepository orderRepository)
    {
            _orderRepository = orderRepository;
    }

    public async Task<Orderviewmodel> GetOrderDetails(int pageNo, int pageSize, string searchKey , string sortBy , string sortDirection  ,string statusFilter ,  string timeFilter ,string fromDate = "" , string toDate=""  )
    {
       var orderData = await _orderRepository.GetOrderDetails( pageNo,  pageSize, searchKey ,  sortBy ,  sortDirection , statusFilter , timeFilter ,  fromDate,  toDate);
       return orderData;
    }

    public async Task<List<Ordertableviewmodel>> GetOrder( string searchKey, string statusFilter, string timeFilter)
    {
        var orderData = await _orderRepository.GetOrder( searchKey,  statusFilter,  timeFilter);
        return orderData;
    }

     public async Task<Order> GetDetails(int orderId)
     {
        return await _orderRepository.GetDetails(orderId);
     }

      // public async Task<Order> ExportToPdf(int orderId)
      // {
      //    var order = await _orderRepository.GetDetails(orderId);
      //    return View(order);
      // }
}

using BAL.Models.Interfaces;
using DAL.Interfaces;
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

}

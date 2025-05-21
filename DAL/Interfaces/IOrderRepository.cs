using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace DAL.Interfaces;

public interface IOrderRepository
{
    public Task<List<Order>> GetOrderDetails(int pageNo, int pageSize,string searchKey , string sortBy , string sortDirection ,string statusFilter , string timeFilter , string fromDate = "" , string toDate="" );
    public Task<List<Order>> GetOrder(string searchKey, string statusFilter, string timeFilter );
    public Task<Order> GetDetails(int orderId);
}

using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace DAL.Interfaces;

public interface IDashboardRepository
{
    public Task<decimal> GetTotalSales(DateTime startDate ,  DateTime endDate );
    public Task<int> GetTotalOrders(DateTime startDate ,  DateTime endDate);
    public Task<decimal> GetAvgOrderValue(DateTime startDate ,  DateTime endDate);
    public Task<decimal> GetAvgWaitingTime(DateTime startDate ,  DateTime endDate);
    public Task<int> GetWaitingListCount(DateTime startDate ,  DateTime endDate);
    public Task<int> GetNewCustomerCount(DateTime startDate ,  DateTime endDate);
    public Task<List<ItemDashboardviewmodel>> GetTopSellingItems(DateTime startDate ,  DateTime endDate);
    public Task<List<ItemDashboardviewmodel>> GetLeastSellingItems(DateTime startDate ,  DateTime endDate);
    public Task<List<Revenueviewmodel>> GetRevenueList(DateTime startDate ,  DateTime endDate);
    public Task<List<CustomerDashboardviewmodel>> GetCustomerList(DateTime startDate ,  DateTime endDate);
}

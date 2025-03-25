using Pizzashop.DAL.ViewModels;

namespace DAL.Interfaces;

public interface IOrderRepository
{
    public Task<Orderviewmodel> GetOrderDetails(int pageNo, int pageSize,string searchKey , string sortBy , string sortDirection ,string statusFilter , string timeFilter , string fromDate = "" , string toDate="" );
}

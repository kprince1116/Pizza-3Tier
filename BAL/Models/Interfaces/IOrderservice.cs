using DAL.Interfaces;
using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace BAL.Models.Interfaces;

public interface IOrderservice 
{
    public Task<Orderviewmodel> GetOrderDetails( int pageNo ,int pageSize , string searchKey , string sortBy , string sortDirection ,string statusFilter , string timeFilter , string fromDate = "" , string toDate="" );
    public Task<List<Ordertableviewmodel>> GetOrder( string searchKey, string statusFilter, string timeFilter );

    public Task<Order> GetDetails(int orderId);

    // public Task<byte[]> ExportToPdf(int orderId);
}

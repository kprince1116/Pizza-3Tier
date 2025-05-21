using System.Data.Common;
using System.Linq.Expressions;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Pizzashop.DAL.ViewModels;

namespace DAL.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly PizzaShopContext _db;

    public OrderRepository(PizzaShopContext db)
    {
        _db = db;
    }

    public async Task<List<Order>> GetOrderDetails(int pageNo, int pageSize,string searchKey,string sortBy , string sortDirection ,string statusFilter , string timeFilter , string fromDate = "" , string toDate="" )
    {
        var orders = await  _db.Orders.Where(u=>u.Isdelete == false).Include(u=>u.Customer).Include(u=>u.RatingNavigation).Include(u=>u.PaymentModeNavigation).Include(u=>u.StatusNavigation).ToListAsync();

        return orders;
        
    }

    public async Task<List<Order>> GetOrder(string searchKey, string statusFilter, string timeFilter)
    {
        var orders =  await _db.Orders.Where(u=>u.Isdelete == false).Include(u=>u.Customer).Include(u=>u.PaymentModeNavigation).Include(u=>u.StatusNavigation).ToListAsync();

        return orders;
        
      
      
    }
    public async Task<Order> GetDetails(int orderId)
    {
        var orderDetails = await _db.Orders.Include(o=>o.Customer)
                                            .Include(o=>o.StatusNavigation)
                                            .Include(o=>o.PaymentModeNavigation)
                                            .Include(o=>o.OrderItems.Where(oi => oi.IsDeleted == false)).ThenInclude(o=>o.Item)
                                            .Include(o=>o.OrderItems) .ThenInclude(o=>o.OrderItemModifiers).ThenInclude(o=>o.Modifier)                                 
                                            .Include(o=>o.OrderTables).ThenInclude(t=>t.Table).ThenInclude(s=>s.Section)
                                            .Include(o=>o.OrderTaxes).ThenInclude(t=>t.Tax)
                                            .FirstOrDefaultAsync(o=>o.Orderid==orderId);

         return orderDetails;
    }


}

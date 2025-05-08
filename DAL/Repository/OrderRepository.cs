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

    public async Task<Orderviewmodel> GetOrderDetails(int pageNo, int pageSize,string searchKey,string sortBy , string sortDirection ,string statusFilter , string timeFilter , string fromDate = "" , string toDate="" )
    {
        var orders =  _db.Orders.Where(u=>u.Isdelete == false).Include(u=>u.Customer).Include(u=>u.RatingNavigation).Include(u=>u.PaymentModeNavigation).Include(u=>u.StatusNavigation).Select
        (
            u => new Ordertableviewmodel
            {
                Orderid = u.Orderid,
                OrderNo = u.OrderNo,
                Orderdate = u.CreatedDate,
                CustomerName = u.Customer.Customername,
                Status = u.StatusNavigation.Status,
                Payment = u.PaymentModeNavigation.Status,
                Rating = u.RatingNavigation.Avgrating,
                TotalAmount = u.TotalAmount,
            }
        );

         if (!string.IsNullOrEmpty(searchKey))
        {
            var lowerSearchQuery = searchKey.ToLower();
            orders = orders.Where(o => o.CustomerName.ToLower().Contains(lowerSearchQuery) ||
                                   o.OrderNo.ToString().ToLower().Contains(lowerSearchQuery) ||
                                   o.Status.ToLower().Contains(lowerSearchQuery));
        }

        if(!string.IsNullOrEmpty(statusFilter))
        {
            orders = orders.Where(u=>u.Status.ToLower() == statusFilter.ToLower());
        }

        if(!string.IsNullOrEmpty(timeFilter))
        {
             DateTime startDate = DateTime.MinValue;
             DateTime endDate = DateTime.MaxValue;

             switch(timeFilter.ToLower())
             {
                case "today":
                startDate = DateTime.Today;
                endDate = DateTime.Today.AddDays(1).AddTicks(-1);
                break;
    
                case "this_week":
                startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                endDate = startDate.AddDays(7).AddTicks(-1);
                break;

                case "this_month":
                startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                endDate = startDate.AddMonths(1).AddTicks(-1);
                break;

             }           
           if (startDate != DateTime.MinValue && endDate != DateTime.MaxValue)
            {
            orders = orders.Where(o =>  o.Orderdate.HasValue && o.Orderdate.Value >= startDate && o.Orderdate.Value <= endDate);
            }
                // orders = orders.Where(o => o.Orderdate >= startDate && o.Orderdate <= endDate);
        }

       
        if(!string.IsNullOrEmpty(fromDate))
        {
              if (DateTime.TryParse(fromDate, out var fromDateTime))
            {
                orders = orders.Where(o => o.Orderdate.HasValue && o.Orderdate.Value.Date >= fromDateTime.Date);
            }
        }

        if(!string.IsNullOrEmpty(toDate))
        {
             if (DateTime.TryParse(toDate, out var toDateTime))
            {
                orders = orders.Where(o => o.Orderdate.HasValue && o.Orderdate.Value.Date <= toDateTime.Date);
            }
        }

         
        switch(sortBy)
        {
            case "OrderNo":
                     orders = (sortDirection == "asc") ? orders.OrderBy(U=>U.OrderNo) : orders.OrderByDescending(u=>u.OrderNo);
                     break;
            case "Orderdate":
                     orders = (sortDirection == "asc") ? orders.OrderBy(U=>U.Orderdate) : orders.OrderByDescending(u=>u.Orderdate);
                     break;
            case "CustomerName":
                     orders = (sortDirection == "asc") ? orders.OrderBy(U=>U.CustomerName) : orders.OrderByDescending(u=>u.CustomerName);
                     break;
            case "TotalAmount":
                     orders = (sortDirection == "asc") ? orders.OrderBy(U=>U.TotalAmount) : orders.OrderByDescending(u=>u.TotalAmount);
                     break;                           
            default:
                     orders = orders.OrderByDescending(U=>U.Orderid);
                     break;
        }
    
        var totalRecords = orders.Count();
    
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
    
        List<Ordertableviewmodel> orderList = orders.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
    
        return new Orderviewmodel
        {
            Orders = orderList,
            CurrentPage = pageNo,
            TotalPages  = totalPages,
            PageSize = pageSize ,
            TotalRecords = totalRecords,
        };
    }

    public async Task<List<Ordertableviewmodel>> GetOrder(string searchKey, string statusFilter, string timeFilter)
    {
        IQueryable<Ordertableviewmodel> orders =   _db.Orders.Where(u=>u.Isdelete == false).Include(u=>u.Customer).Include(u=>u.PaymentModeNavigation).Include(u=>u.StatusNavigation).Select
        (
            u => new Ordertableviewmodel
            {
                Orderid = u.Orderid,
                OrderNo = u.OrderNo,
                Orderdate = u.Orderdate,
                CustomerName = u.Customer.Customername,
                Status = u.StatusNavigation.Status,
                Payment = u.PaymentModeNavigation.Status,
                Rating = u.Rating,
                TotalAmount = u.TotalAmount,
            }
        ).OrderBy(u=>u.Orderid);
    
        if (!string.IsNullOrEmpty(searchKey))
        {
            var lowerSearchQuery = searchKey.ToLower();
            orders = orders.Where(o => o.CustomerName.ToLower().Contains(lowerSearchQuery) ||
                                   o.OrderNo.ToString().ToLower().Contains(lowerSearchQuery) ||
                                   o.Status.ToLower().Contains(lowerSearchQuery));
        }
    
        if(!string.IsNullOrEmpty(statusFilter))
        {
            orders = orders.Where(u=>u.Status.ToLower() == statusFilter.ToLower());
        }
    
        if(!string.IsNullOrEmpty(timeFilter))
        {
             DateTime startDate = DateTime.MinValue;
             DateTime endDate = DateTime.MaxValue;
    
             switch(timeFilter.ToLower())
             {
                case "today":
                startDate = DateTime.Today;
                endDate = DateTime.Today.AddDays(1).AddTicks(-1);
                break;
    
                case "this_week":
                startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                endDate = startDate.AddDays(7).AddTicks(-1);
                break;
    
                case "this_month":
                startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                endDate = startDate.AddMonths(1).AddTicks(-1);
                break;
    
             }  
                      
           if (startDate != DateTime.MinValue && endDate != DateTime.MaxValue)
            {
               orders = orders.Where(o =>  o.Orderdate.HasValue && o.Orderdate.Value >= startDate && o.Orderdate.Value <= endDate);
            }
        }
    
         return await orders.ToListAsync();
    }
    public async Task<Order> GetDetails(int orderId)
    {
        var orderDetails = await _db.Orders.Include(o=>o.Customer)
                                            .Include(o=>o.StatusNavigation)
                                            .Include(o=>o.PaymentModeNavigation)
                                            .Include(o=>o.OrderItems).ThenInclude(o=>o.Item)
                                            .Include(o=>o.OrderItems) .ThenInclude(o=>o.OrderItemModifiers).ThenInclude(o=>o.Modifier)                                 
                                            .Include(o=>o.OrderTables).ThenInclude(t=>t.Table).ThenInclude(s=>s.Section)
                                            .Include(o=>o.OrderTaxes).ThenInclude(t=>t.Tax)
                                            .FirstOrDefaultAsync(o=>o.Orderid==orderId);

         return orderDetails;
    }


}

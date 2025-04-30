using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Pizzashop.DAL.ViewModels;

namespace DAL.Repository;

public class DashboardRepository : IDashboardRepository
{
    private readonly PizzaShopContext _db;

    public DashboardRepository(PizzaShopContext db)
    {
        _db = db;
    }
    public async Task<int> GetTotalOrders(DateTime startDate ,  DateTime endDate )
    {
        try
        {
          var orders = _db.Orders.Where(u=>u.Status == 2 && u.Isdelete == false &&  u.Orderdate.HasValue && u.Orderdate.Value >= startDate && u.Orderdate.Value <= endDate);
    
        int totalorder = await orders.CountAsync();
    
        return totalorder;   
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return 0;
        }
       
    }

    public async Task<decimal> GetTotalSales(DateTime startDate ,  DateTime endDate )
    {
        try
        {
        var totalsales = _db.Paymentmodes.Where(p=>p.PaymentStatus == "paid" && p.PaidOn.HasValue && p.PaidOn.Value >= startDate && p.PaidOn.Value <= endDate);

        
             decimal totalamount = 0;

             totalamount = (decimal) await totalsales.SumAsync(u=>u.Totalamount);
            return totalamount;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return 0;
        }
    }


    public async Task<decimal> GetAvgOrderValue(DateTime startDate ,  DateTime endDate )
    {
        try
        {
             var totalsales = _db.Paymentmodes.Where(p=>p.PaymentStatus == "paid" &&  p.PaidOn.HasValue && p.PaidOn.Value >= startDate && p.PaidOn.Value <= endDate );


        decimal avgamount = 0;

        avgamount = Math.Round((decimal) await totalsales.AverageAsync(u=>u.Totalamount),2);

        return avgamount;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return 0;
        }
        
    }

    public async Task<decimal> GetAvgWaitingTime(DateTime startDate ,  DateTime endDate )
    {
        try
        {
             var waiting = _db.WaitingTokens.Where(u=>u.IsAssigned == true &&  u.CreatedDate.HasValue && u.CreatedDate.Value >= startDate && u.CreatedDate.Value <= endDate );

         var waitinglist = await waiting.Select(u => (u.AssignedTime.Value - u.CreatedDate.Value).TotalMinutes).ToListAsync();

        decimal avgwaitingtime = 0;

        avgwaitingtime =Math.Round((decimal) waitinglist.Average(),2);

        return avgwaitingtime;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return 0;
        }
       
    }

    public async Task<int> GetNewCustomerCount(DateTime startDate ,  DateTime endDate)
    {
        try
        {
            var Customers = _db.Customers.Where(u=>u.Isdelete == false && u.CreatedDate.HasValue && u.CreatedDate.Value >= startDate && u.CreatedDate.Value <= endDate );


        int customercount = await Customers.CountAsync();

        return customercount;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return 0;
        }
        
    }

   
    public async Task<int> GetWaitingListCount(DateTime startDate ,  DateTime endDate)
    {
        try
        {
             var waitingcount = _db.WaitingTokens.Where(u=>u.IsDeleted == false && u.CreatedDate.HasValue && u.CreatedDate.Value >= startDate && u.CreatedDate.Value <= endDate );

        int totalawaiting = await waitingcount.CountAsync();
    
        return totalawaiting;
        }
        catch (Exception e)
        {
            return 0;
        }
    }

    public async Task<List<OrderItem>> GetItemList(DateTime startDate ,  DateTime endDate)
    {
        return null;
    }


    public async Task<List<ItemDashboardviewmodel>> GetTopSellingItems(DateTime startDate, DateTime endDate)
    {
       var orderItem = _db.OrderItems.Include(u=>u.Item).Include(u=>u.Order).Where(u=> u.CreatedDate.HasValue && u.CreatedDate.Value >= startDate && u.CreatedDate.Value <= endDate);
        
        var itemList = orderItem.GroupBy(u=>u.ItemId).Select
       (u=>new ItemDashboardviewmodel{
            ItemId = (int) u.First().ItemId,
            ItemName = u.First().Item.Itemname,
            OrderCount = u.Count(),
            ItemImage = u.First().Item.Image
       }).OrderByDescending(u=>u.OrderCount).Take(2).ToList();
    
       return itemList;
    }
    public async Task<List<ItemDashboardviewmodel>> GetLeastSellingItems(DateTime startDate, DateTime endDate)
    {
       var orderItem = _db.OrderItems.Include(u=>u.Item).Include(u=>u.Order).Where(u=> u.CreatedDate.HasValue && u.CreatedDate.Value >= startDate && u.CreatedDate.Value <= endDate);
        
        var itemList = orderItem.GroupBy(u=>u.ItemId).Select
       (u=>new ItemDashboardviewmodel{
            ItemId = (int) u.First().ItemId,
            ItemName = u.First().Item.Itemname,
            OrderCount = u.Count(),
            ItemImage = u.First().Item.Image
       }).OrderBy(u=>u.OrderCount).Take(2).ToList();
    
       return itemList;
    }

      public async Task<List<Revenueviewmodel>> GetRevenueList(DateTime startDate, DateTime endDate)
      {
        var payments = _db.Paymentmodes.Where(u=> u.PaymentStatus == "paid" && u.PaidOn.HasValue && u.PaidOn.Value >= startDate && u.PaidOn.Value <= endDate).OrderBy(u=>u.PaidOn);

        var revenuelist = payments.Select(u=>new Revenueviewmodel{
            RevenueDate = (DateTime) u.PaidOn,
            TotalRevenue = (decimal) u.Totalamount
        }).ToList();

        return revenuelist;
      }

    public async Task<List<CustomerDashboardviewmodel>> GetCustomerList(DateTime startDate ,  DateTime endDate)
    {
        var customers = _db.Customers.Where(u=> u.CreatedDate.HasValue && u.CreatedDate.Value >= startDate && u.CreatedDate.Value <= endDate).OrderBy(u=>u.CreatedDate);
    
        // var customerCount = await customers.CountAsync();
    
        var customerlist = customers.GroupBy(u=>u.CreatedDate).Select(u=>new CustomerDashboardviewmodel{
            CustomerDate = (DateTime) u.First().CreatedDate,
            TotalCustomer = u.Count()
        }).ToList();
    
        return customerlist;
    }
    

}

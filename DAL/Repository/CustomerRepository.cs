using System.Data.Common;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Pizzashop.DAL.ViewModels;

namespace DAL.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly PizzaShopContext _db;

    public CustomerRepository(PizzaShopContext db)
    {
        _db = db;
    }


    public async Task<Customerviewmodel> GetCustomerDetails(int pageNo,int pageSize,string searchKey,string sortby , string sortdirection , string timefilter,string fromdate , string todate)
    {
        var customers = _db.Customers.Where(u=>u.Isdelete == false).Select(
            u=> new CustomerDetailsviewmodel
            {
                Customerid = u.Customerid,
                Customername = u.Customername,
                Customeremail = u.Customeremail,
                Phonenumber = u.Phonenumber,
                CutomerDate = u.CreatedDate,
                TotalOrders = u.Orders.Count(),
            }
        );

        if (!string.IsNullOrWhiteSpace(searchKey))
        {
          var lowerSearchQuery = searchKey.Trim().ToLower();
                 customers = customers.Where(o => 
                (!string.IsNullOrEmpty(o.Customeremail) && o.Customeremail.ToLower().Contains(lowerSearchQuery)) ||
                (!string.IsNullOrEmpty(o.Customername) && o.Customername.ToLower().Contains(lowerSearchQuery)) ||
                (!string.IsNullOrEmpty(o.Phonenumber) && o.Phonenumber.ToLower().Contains(lowerSearchQuery))
        );  
        }

        switch(sortby)
        {
            case "Customername":
                customers = (sortdirection == "asc") ? customers.OrderBy(c=>c.Customername) : customers.OrderByDescending(c=>c.Customername);
                break;

            case "CutomerDate":
                customers = (sortdirection == "asc") ? customers.OrderBy(c=>c.CutomerDate) : customers.OrderByDescending(c=>c.CutomerDate);
                break;   

            case "TotalOrders" :
                customers = (sortdirection == "asc") ? customers.OrderBy(c=>c.TotalOrders) : customers.OrderByDescending(c=>c.TotalOrders);
                break;

             default:
             customers = customers.OrderBy(c=>c.Customerid);
             break;
        }

        if(!string.IsNullOrEmpty(timefilter))
        {
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;

            switch (timefilter.ToLower())
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
            if(startDate != DateTime.MinValue && endDate != DateTime.MaxValue)
            {
                customers = customers.Where(c=>c.CutomerDate.HasValue && c.CutomerDate.Value >= startDate && c.CutomerDate.Value<=endDate);
            }

        }

        if(!string.IsNullOrEmpty(fromdate))
        {
            if(DateTime.TryParse(fromdate,out var fromDateTime))
            {
                customers = customers.Where(c=>c.CutomerDate.HasValue && c.CutomerDate.Value.Date >= fromDateTime.Date);
            }

            
        }

        if(!string.IsNullOrEmpty(todate))
        {
            if(DateTime.TryParse(todate,out var toDateTime))
            {
                customers = customers.Where(c=>c.CutomerDate.HasValue && c.CutomerDate.Value.Date <= toDateTime.Date);
            }
        }

        var totalRecords = customers.Count();
    
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
    
        List<CustomerDetailsviewmodel> customerList = customers.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
    
        return new Customerviewmodel
        {
            Customers = customerList,
            CurrentPage = pageNo,
            TotalPages  = totalPages,
            PageSize = pageSize ,
            TotalRecords = totalRecords,
        };
    }


    public async Task<List<CustomerDetailsviewmodel>> GetCustomer(string searchKey, string timefilter ,  string fromdate , string todate)
    {
         var customers = _db.Customers.Where(u=>u.Isdelete == false).Select(
            u=> new CustomerDetailsviewmodel
            {
                Customerid = u.Customerid,
                Customername = u.Customername,
                Customeremail = u.Customeremail,
                Phonenumber = u.Phonenumber,
                CutomerDate = u.CreatedDate,
                TotalOrders = u.Orders.Count(),
            }
        );

         if (!string.IsNullOrWhiteSpace(searchKey))
        {
          var lowerSearchQuery = searchKey.Trim().ToLower();
                 customers = customers.Where(o => 
                (!string.IsNullOrEmpty(o.Customeremail) && o.Customeremail.ToLower().Contains(lowerSearchQuery)) ||
                (!string.IsNullOrEmpty(o.Customername) && o.Customername.ToLower().Contains(lowerSearchQuery)) ||
                (!string.IsNullOrEmpty(o.Phonenumber) && o.Phonenumber.ToLower().Contains(lowerSearchQuery))
        );  
        }

        if(!string.IsNullOrEmpty(timefilter))
        {
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;

            switch (timefilter.ToLower())
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
            if(startDate != DateTime.MinValue && endDate != DateTime.MaxValue)
            {
                customers = customers.Where(c=>c.CutomerDate.HasValue && c.CutomerDate.Value >= startDate && c.CutomerDate.Value<=endDate);
            }

             if(!string.IsNullOrEmpty(fromdate))
        {
            if(DateTime.TryParse(fromdate,out var fromDateTime))
            {
                customers = customers.Where(c=>c.CutomerDate.HasValue && c.CutomerDate.Value.Date >= fromDateTime.Date);
            }

            
        }

        if(!string.IsNullOrEmpty(todate))
        {
            if(DateTime.TryParse(todate,out var toDateTime))
            {
                customers = customers.Where(c=>c.CutomerDate.HasValue && c.CutomerDate.Value.Date <= toDateTime.Date);
            }
        }


        }

        return await customers.ToListAsync();

    }

    public async Task<Customer> GetCustomerHistory(int id)
    {
        return await _db.Customers.Include(c=>c.Orders).ThenInclude(p=>p.PaymentModeNavigation)
                                .Include(C=>C.Orders).ThenInclude(O=>O.OrderItems).FirstOrDefaultAsync(U=>U.Customerid == id);
    }

}

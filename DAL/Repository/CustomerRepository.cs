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


    public async Task<List<Customer>> GetCustomerDetails(int pageNo,int pageSize,string searchKey,string sortby , string sortdirection , string timefilter,string fromdate , string todate)
    {
        var customers = await _db.Customers.Include(u=>u.Orders).Where(u=>u.Isdelete == false).ToListAsync();

        return customers;
        
       
    }


    public async Task<List<Customer>> GetCustomer(string searchKey, string timefilter ,  string fromdate , string todate)
    {
         var customers = await _db.Customers.Include(u=>u.Orders).Where(u=>u.Isdelete == false).ToListAsync();
         return customers;
         
        
    }


    

    public async Task<Customer> GetCustomerHistory(int id)
    {
        return await _db.Customers.Include(c=>c.Orders).ThenInclude(p=>p.PaymentModeNavigation)
                                .Include(C=>C.Orders).ThenInclude(O=>O.OrderItems).FirstOrDefaultAsync(U=>U.Customerid == id);
    }

}

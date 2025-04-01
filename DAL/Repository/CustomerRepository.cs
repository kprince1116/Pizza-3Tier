using DAL.Interfaces;
using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace DAL.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly PizzaShopContext _db;

    public CustomerRepository(PizzaShopContext db)
    {
        _db = db;
    }

    public async Task<Customerviewmodel> GetCustomerDetails(int pageNo,int pageSize,string searchKey)
    {
        var customers = _db.Customers.Where(u=>u.Isdelete == false).Select(
            u=> new CustomerDetailsviewmodel
            {
                Customerid = u.Customerid,
                Customername = u.Customername,
                Customeremail = u.Customeremail,
                Phonenumber = u.Phonenumber,
                CutomerDate = u.CutomerDate,
                TotalOrders = u.TotalOrders,
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

}

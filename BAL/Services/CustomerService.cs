using System.ComponentModel;
using BAL.Models.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace BAL.Services;

public class CustomerService : ICustomerService
{

    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customerviewmodel> GetCustomerDetails(int pageNo,int pageSize,string searchKey,string sortby , string sortdirection ,string timefilter,string fromdate , string todate )
    {
        var customer = await _customerRepository.GetCustomerDetails( pageNo, pageSize, searchKey , sortby , sortdirection , timefilter , fromdate , todate);
        return customer;
    }

     public async Task<List<CustomerDetailsviewmodel>> GetCustomer(string searchKey,string timefilter, string fromdate, string todate)
     {
         var customer = await _customerRepository.GetCustomer(searchKey,timefilter,fromdate,todate);
        return customer;
     }

     public async Task<CustomerHistoryviewmodel> GetCustomerHistory(int id)
     {
        try
        {
             var customer = await _customerRepository.GetCustomerHistory(id);

        var viewmodel = new CustomerHistoryviewmodel
        {
            comingsince = (DateTime) customer.CreatedDate,
            CustomerName = customer.Customername,
            Phonenumber = customer.Phonenumber,
            totalvisits = customer.Orders.Count, 
            avgBill = customer.Orders.Any() 
            ? (decimal)customer.Orders.Where(u => u.PaymentMode != null).Sum(u => u.TotalAmount) / customer.Orders.Count 
            : 0,
             maxOrder = customer.Orders.Any(u => u.PaymentMode != null) 
            ? (decimal)customer.Orders.Where(u => u.PaymentMode != null).Select(u => u.TotalAmount).Max() 
            : 0,

            OrderList = customer.Orders.Select(u => new Orderdata
            {
                OrderDate = (DateTime) u.Orderdate,
                OrderType = "Dineln",
                paymentmode = u.PaymentModeNavigation.Status,
                totalamount = (decimal) u.TotalAmount,
                totalitems = u.OrderItems.Count()
            }).ToList()
           
        };
        return viewmodel;
        }
        catch (Exception e)
        {
            return null;
        }
       
     }

}

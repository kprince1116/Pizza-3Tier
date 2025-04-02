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

     public async Task<Customer> GetCustomerHistory(int id)
     {
        var customer = await _customerRepository.GetCustomerHistory(id);
        return customer;
     }

}

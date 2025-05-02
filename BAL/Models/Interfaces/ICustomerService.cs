using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace BAL.Models.Interfaces;

public interface ICustomerService
{
    public Task<Customerviewmodel> GetCustomerDetails(int pageNo,int pageSize,string searchKey,string sortby , string sortdirection,string timefilter , string fromdate, string todate);

    public Task<List<CustomerDetailsviewmodel>> GetCustomer(string searchKey,string timefilter, string fromdate , string todate);

    public Task<CustomerHistoryviewmodel> GetCustomerHistory(int id);
}

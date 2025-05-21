using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace DAL.Interfaces;

public interface ICustomerRepository
{
    public Task<List<Customer>> GetCustomerDetails(int pageNo,int pageSize,string searchKey,string sortby , string sortdirection , string timefilter, string fromdate , string todate);

    public Task<List<Customer>> GetCustomer(string searchKey,string timefilter, string fromdate, string todate);

    public Task<Customer> GetCustomerHistory(int id);
}

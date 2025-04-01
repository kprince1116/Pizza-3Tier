using Pizzashop.DAL.ViewModels;

namespace DAL.Interfaces;

public interface ICustomerRepository
{
    public Task<Customerviewmodel> GetCustomerDetails(int pageNo,int pageSize,string searchKey);
}

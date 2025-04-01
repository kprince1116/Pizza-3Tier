using Pizzashop.DAL.ViewModels;

namespace BAL.Models.Interfaces;

public interface ICustomerService
{
    public Task<Customerviewmodel> GetCustomerDetails(int pageNo,int pageSize,string searchKey);
}

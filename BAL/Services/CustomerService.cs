using BAL.Models.Interfaces;
using DAL.Interfaces;
using Pizzashop.DAL.ViewModels;

namespace BAL.Services;

public class CustomerService : ICustomerService
{

    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customerviewmodel> GetCustomerDetails(int pageNo,int pageSize,string searchKey)
    {
        var customer = await _customerRepository.GetCustomerDetails( pageNo, pageSize, searchKey);
        return customer;
    }

}

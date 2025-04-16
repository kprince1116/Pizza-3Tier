using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace BAL.Models.Interfaces;

public interface IKotTableService
{
    Task<List<KotTableviewmodel>> GetSections();

    Task<bool> AddWaitingToken(waitingtokenviewmodel model);

    Task<List<Section>> GetSectionList();

    Task<List<waitingtokenviewmodel>> GetWaitingList(int id);

    Task<CustomerDetailsForTableviewmodel> GetCustomerDetails(int id);

    Task<CustomerDetailsForTableviewmodel> GetCustomerDetailsByEmail(int sectionid,string email);

    Task<bool> AssignTable(waitingtokenviewmodel model);
}

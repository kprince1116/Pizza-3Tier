using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace DAL.Interfaces;

public interface IWaitingRepository
{
    Task<List<Section>> GetSections();

    Task<List<waitingtokenviewmodel>> GetWaitingList(int sectionId);

    public  Task<Customer> GetCustomerDetailsByEmail(string email);

    Task<waitingtokenviewmodel> EditToken(int id);

    Task<WaitingToken> GetTokenId(int id);

    Task Update(WaitingToken waitingToken);

    Task<List<Section>> GetTableDetails();

    Task<List<Table>> GetTablesBySectionId(int id);

    Task<WaitingToken> GetCustomerById(int id);

    Task<Customer> GetCustomer(int id);

    Task<Table> GetTableBySectionId(int id);

    Task UpdateTable(Table tables);

    Task UpdateCustomer( WaitingToken customer);

    Task UpdateCustomers (Customer customer);

}

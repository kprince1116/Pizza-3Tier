using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace DAL.Interfaces;

public interface IKotTableRepository
{
     Task<List<Section>> GetSections();

    Task<List<Table>> GetTablesBySectionIdAsync(int sectionId);

    Task AddWaitingToken(waitingtokenviewmodel model);

    Task<List<Section>> GetSectionList();

    Task<List<waitingtokenviewmodel>> GetWaitingList(int id);

    Task<WaitingToken> GetCustomerDetails(int id);

    Task<Customer> GetCustomerDetailsByEmail(string email);

    Task<WaitingToken> GetCustomerDetailsForAssign(int customerId);

    Task UpdateTables(Table tables);


    Task AddCustomer(Customer newcustomer);

    Task UpdateCustomer( WaitingToken model);

    Task<Table> GetTablesByIdAsync(int tableId);

    Task<Customer> GetCustomerFromCustomerTable(int id);

    Task<Order> GenerateOrder(Order obj);

    Task<OrderTable> AddOrderTable(OrderTable orderTable);
}

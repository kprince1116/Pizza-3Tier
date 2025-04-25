using DAL.Models;
using DAL.Repository;
using Pizzashop.DAL.ViewModels;

namespace DAL.Interfaces;

public interface IOrderAppMenuRepository 
{
    Task<List<MenuCategory>> GetCategories();

    Task<List<MenuItem>> GetItems(int CategoryId,string SearchKey);

    Task<List<MenuItem>> GetFavouriteItems(string SearchKey);

    Task<MenuItem> GetItemById(int ItemId);

    Task UpdateItem(MenuItem item);

    Task<MenuItem> GetItemForModalById(int ItemId);

    Task<Order> GetTableData( int OrderId);

    Task<List<OrderItem>> GetOrderItems(int OrderId);

    Task<Order> GetCustomerDetails(int OrderId);

    Task<Customer> GetCustomerById(int CustomerId);

    Task<Order> GetOrderDetails(int OrderId);

    Task UpdateCustomer(Customer customer);

    Task UpdateOrder(Order order);

    Task<Order> GetOrderComments(int OrderId);

    Task<List<Taxesandfess>> GetTaxList();

    Task<List<OrderTax>> GetTaxLists(int OrderId);

}

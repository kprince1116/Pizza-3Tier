using DAL.Models;
using DAL.Repository;

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
}

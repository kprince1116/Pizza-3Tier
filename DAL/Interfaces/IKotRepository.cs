using DAL.Models;

namespace DAL.Interfaces;

public interface IKotRepository
{
    Task<List<MenuCategory>> GetCategories();

    Task<List<Order>> GetOrders();
}

using DAL.Models;
using DAL.Repository;

namespace DAL.Interfaces;

public interface IOrderAppMenuRepository 
{
    Task<List<MenuCategory>> GetCategories();

    Task<List<MenuItem>> GetItems(int CategoryId,string SearchKey);
}

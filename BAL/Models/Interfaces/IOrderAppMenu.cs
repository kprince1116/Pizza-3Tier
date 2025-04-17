using Pizzashop.DAL.ViewModels;

namespace BAL.Models.Interfaces;

public interface IOrderAppMenu
{
    public Task<OrderAppMenuviewmodel> GetCategories();
    public Task<OrderAppMenuviewmodel> GetItems(int CategoryId , string SearchKey);

   
}

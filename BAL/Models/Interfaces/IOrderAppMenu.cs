using DAL.ViewModels;
using Pizzashop.DAL.ViewModels;

namespace BAL.Models.Interfaces;

public interface IOrderAppMenu
{
    public Task<OrderAppMenuviewmodel> GetCategories();
    public Task<OrderAppMenuviewmodel> GetItems(int CategoryId , string SearchKey);
    public Task<OrderAppMenuviewmodel> GetFavouriteItems(string SearchKey);
    public Task<bool> UpdateFavourite(int ItemId);
    public Task<MenuItemModalviewmoel> GetModalData(int ItemId);
    public Task<OrderAppMenuviewmodel> GetOrderData(int OrderId);
}

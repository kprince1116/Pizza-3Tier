using DAL.Models;
using DAL.ViewModels;
using Pizzashop.DAL.ViewModels;

namespace BAL.Models.Interfaces;

public interface IKotService
{
    // public Task<Kotviewmodel> GetCategories();

    public Task<Kotviewmodel> GetCategoriesFromFunctionAsync();
    public Task<Kotviewmodel> GetKotDataAsync(string status,int categoryId);
    public Task<KOTVM> GetKotData(string status,int categoryId);
    // public Task<OrderCardviewmodel> GetKotDetailsAsync(int id,string status);
    public Task<OrderCardviewmodel> GetKotCardData(int id,string status,int CategoryId);
    public Task<bool> UpdateQuantityAsync(int orderId, string status ,int itemId,int OrderItemId , int quantity);

}

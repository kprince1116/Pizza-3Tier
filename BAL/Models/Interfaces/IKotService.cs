using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace BAL.Models.Interfaces;

public interface IKotService
{
    public Task<Kotviewmodel> GetKotDataAsync(string status,int categoryId);

    public Task<OrderCardviewmodel> GetKotDetailsAsync(int id);

    public Task<bool> UpdateQuantityAsync(int orderId,int itemId, int quantity);

}

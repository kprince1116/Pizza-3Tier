using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace BAL.Models.Interfaces;

public interface IKotService
{
    public Task<Kotviewmodel> GetKotDataAsync(string status,int categoryId);

    public Task<OrderCardviewmodel> GetKotDetailsAsync(int id,string status);

    public Task<bool> UpdateQuantityAsync(int orderId, string status ,int itemId, int quantity);

}

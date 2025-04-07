using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace DAL.Interfaces;

public interface IKotRepository
{
        Task<Kotviewmodel> GetKotDataAsync(string status,int categoryId);

        Task<OrderCardviewmodel> GetKotDetailsAsync(int id);

        Task<bool> UpdateQuantityAsync(int orderId,int itemId, int quantity);
}

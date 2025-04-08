using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace DAL.Interfaces;

public interface IKotRepository
{
        Task<Kotviewmodel> GetKotDataAsync(string status,int categoryId);

        Task<OrderCardviewmodel> GetKotDetailsAsync(int id,string status);

        Task<bool> UpdateQuantityAsync(int orderId,string status,int itemId, int quantity);
}

using DAL.Models;
using DAL.ViewModels;
using Pizzashop.DAL.ViewModels;

namespace DAL.Interfaces;

public interface IKotRepository
{

        Task<List<MenuCategory>> GetCategories();
        Task<List<MenuCategory>> GetCategoriesFromFunctionAsync();
        Task<Kotviewmodel> GetKotDataAsync(string status,int categoryId);
        Task<List<OrderDtoViewmodel>> GetKotData(string status , int categoryId);
        Task<OrderCardviewmodel> GetKotDetailsAsync(int id,string status);
        Task<List<KotOrderCardDTOViewModel>> GetKotCardData(int id,string status , int CategoryId);
        Task<bool> UpdateQuantityAsync(int orderId,string status,int itemId, int quantity);
}

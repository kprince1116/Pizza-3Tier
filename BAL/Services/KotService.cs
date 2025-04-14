using BAL.Models.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace BAL.Services;

public class KotService : IKotService
{
    private readonly IKotRepository _kotRepository;

    public KotService(IKotRepository kotRepository)
    {
        _kotRepository = kotRepository;
    }
    
    public async Task<Kotviewmodel> GetCategories()
    {
        var category = await _kotRepository.GetCategories();
        var categories = new Kotviewmodel
        {
            Categories = category,
        };
        return categories;
    }
    public async Task<Kotviewmodel> GetKotDataAsync(string status,int categoryId)
    {
        return await _kotRepository.GetKotDataAsync(status,categoryId);
    }

     public async Task<OrderCardviewmodel> GetKotDetailsAsync(int id,string status)
     {
        return await _kotRepository.GetKotDetailsAsync(id,status);
     }

    public async Task<bool> UpdateQuantityAsync(int orderId, string status ,int itemId, int quantity)
    {
        var result = await _kotRepository.UpdateQuantityAsync(orderId,status,itemId, quantity);
        return result;
    }
}

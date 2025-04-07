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

    public async Task<Kotviewmodel> GetKotDataAsync(string status,int categoryId)
    {
        return await _kotRepository.GetKotDataAsync(status,categoryId);
    }

     public async Task<OrderCardviewmodel> GetKotDetailsAsync(int id)
     {
        return await _kotRepository.GetKotDetailsAsync(id);
     }

    public async Task<bool> UpdateQuantityAsync(int orderId,int itemId, int quantity)
    {
        var result = await _kotRepository.UpdateQuantityAsync(orderId,itemId, quantity);
        return result;
    }
}

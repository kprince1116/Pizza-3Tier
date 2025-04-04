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
        var categories = await _kotRepository.GetCategories();
         var model = new Kotviewmodel
         {
                Categories = categories
         };
         return model;
    }

    public async Task<cardviewmodel> GetOrders()
    {
        var orders = await _kotRepository.GetOrders();
        var model = new cardviewmodel
        {
            orders = orders
        };
        return model;
    }
}

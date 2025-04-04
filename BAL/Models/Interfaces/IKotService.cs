using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace BAL.Models.Interfaces;

public interface IKotService
{
    public Task<Kotviewmodel> GetCategories();

    public Task<cardviewmodel> GetOrders();
}

using BAL.Models.Interfaces;
using DAL.Interfaces;
using Pizzashop.DAL.ViewModels;

namespace BAL.Services;

public class OrderAppMenu : IOrderAppMenu
{
    private readonly IOrderAppMenuRepository _orderAppMenuRepository;
    public OrderAppMenu(IOrderAppMenuRepository orderAppMenuRepository)
    {
        _orderAppMenuRepository = orderAppMenuRepository;
    }

    public async Task<OrderAppMenuviewmodel> GetCategories()
    {
        var category = await _orderAppMenuRepository.GetCategories();

        var viewmodel = new OrderAppMenuviewmodel
        {
            categories = category.Select(c => new MenuCategoryviewmodel
            {
                CategoryId = c.Categoryid,
                CategoryName = c.Name
            }).ToList()
        };
        return viewmodel;
    }

    public async Task<OrderAppMenuviewmodel> GetItems(int CategoryId,string SearchKey)
    {
        try
        {
             var items = await _orderAppMenuRepository.GetItems(CategoryId,SearchKey);

        var viewmodel = new OrderAppMenuviewmodel
        {
            items = items.Select(i => new Itemsviewmodel
            {
                ItemId = i.Itemid ,
                ItemName = i.Itemname,
                ItemType = i.Itemtype,
                price = (int)i.Rate,
                isFavourite = (bool) i.IsFavourite
            }).ToList()
        };
        return viewmodel;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
       
    }

    public async Task<OrderAppMenuviewmodel> GetFavouriteItems(string SearchKey)
    {
        var items = await _orderAppMenuRepository.GetFavouriteItems(SearchKey);

        var viewmodel = new OrderAppMenuviewmodel
        {
            items = items.Select(i => new Itemsviewmodel
            {
                ItemId = i.Itemid,
                ItemName = i.Itemname,
                ItemType = i.Itemtype,
                price = (int)i.Rate,
                isFavourite = (bool) i.IsFavourite
            }).ToList()
        };
        return viewmodel;
    }

}

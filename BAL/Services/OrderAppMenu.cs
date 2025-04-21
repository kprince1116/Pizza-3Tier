using BAL.Models.Interfaces;
using DAL.Interfaces;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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

    public async Task<bool> UpdateFavourite(int ItemId)
    {
        var item = await _orderAppMenuRepository.GetItemById(ItemId);

        if(item == null)
        {
            return false;
        }

        if(item.IsFavourite == true)
        {
            item.IsFavourite = false;
        }
        else
        {
            item.IsFavourite = true;
        }
        
        await _orderAppMenuRepository.UpdateItem(item);

        return true;
    }

    public async Task<MenuItemModalviewmoel> GetModalData(int ItemId)
    {
        var item = await _orderAppMenuRepository.GetItemForModalById(ItemId);

        if(item == null)
        {
            return null;
        }

        var viewmodel = new MenuItemModalviewmoel
        {
            ItemId = item.Itemid,
            ItemName = item.Itemname,
           ModifierGroupList = item.Itemmodifiergroups.Select(u => new MenuModifierGroupViewModel
            {
                ModifierGroupId = (int) u.Modifiergroupid,
                ModifierGroupName = u.Modifiergroup.Name,
                Min = (int)u.Minallowed,
                Max = (int) u.Maxallowed,
                ModifierList = u.Modifiergroup.Modifiermappings.Select(v=> new MenuModifierViewModel{
                    ModifierId = (int)v.Modifier.Modifierid,
                    ModifierName = v.Modifier.Modifiername,
                    Price = (decimal)v.Modifier.Rate
               }).ToList()
            }).ToList()
        };
        return viewmodel;
    }

}

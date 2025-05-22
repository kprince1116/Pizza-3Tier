using BAL.Models.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
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

    public async Task<Kotviewmodel> GetCategoriesFromFunctionAsync()
    {
        var category = await _kotRepository.GetCategoriesFromFunctionAsync();
        var categories = new Kotviewmodel
        {
            Categories = category,
        };
        return categories;
    }
    public async Task<Kotviewmodel> GetKotDataAsync(string status, int categoryId)
    {
        return await _kotRepository.GetKotDataAsync(status, categoryId);
    }

    public async Task<KOTVM> GetKotData(string status, int categoryId)
    {
        var orders = await _kotRepository.GetKotData(status, categoryId);

        var model = new KOTVM
        {
            CategoryId = categoryId,
            Getkotdata = orders.GroupBy(u => u.OrderId).Select(u => new KOTVM.GetKotDataViewModel
            {
                OrderId = u.FirstOrDefault().OrderId,
                OrderNo = u.FirstOrDefault().OrderNo,
                OrderDate = u.FirstOrDefault().OrderDate,
                OrderInstruction = u.FirstOrDefault().OrderInstruction,
                OrderStatus = u.FirstOrDefault().OrderStatus,
                TableName = u.FirstOrDefault().TableNo,
                SectionName = u.FirstOrDefault().SectionName,
                ItemLists = u.GroupBy(s => s.OrderItemId).Select(k => new KOTVM.GetKotDataViewModel.OrderItemViewModels
                {
                    ItemName = k.FirstOrDefault().ItemName,
                    Quantity = k.FirstOrDefault().Quantity,
                    PendigQuantity = k.FirstOrDefault().PendingQuantity,
                    ReadyQuantity = k.FirstOrDefault().ReadyQuantity,
                    ItemInstrucion = k.FirstOrDefault().ItemInstruction,
                    Modifiers = k.Select(m => new KOTVM.GetKotDataViewModel.ModifierViewModels
                    {
                        ModifierName = m.ModifierName
                    }).ToList()
                }).OrderBy(u=>u.ItemName).ToList()
            }).ToList()
        };

        return model;

    }

    public async Task<OrderCardviewmodel> GetKotDetailsAsync(int id, string status)
    {
        return await _kotRepository.GetKotDetailsAsync(id, status);
    }

    public async Task<OrderCardviewmodel> GetKotCardData(int id, string status , int CategoryId)
    {
        try
        {
              var order = await _kotRepository.GetKotCardData(id, status ,CategoryId);

        var Model = new OrderCardviewmodel
        {
            OrderID = order.FirstOrDefault().orderid,
            OrderNo = order.FirstOrDefault().orderno,
            OrderStatus = status,
            ItemList = order.GroupBy(u=>u.itemid).Select(u=> new OrderCardviewmodel.OrderItemsviewmodel
            {
                ItemId = u.FirstOrDefault().itemid,
                ItemName = u.FirstOrDefault().itemname,
                OrderItemID = u.FirstOrDefault().orderitemid,
                TotalQuantity = u.FirstOrDefault().totalquantity,
                IsSelected = true,
                PendigQuantity = u.FirstOrDefault().pendingquantity,
                ReadyQuantity = u.FirstOrDefault().readyquantity,
                ModifierList = u.Select(m => new OrderCardviewmodel.OrderItemModifierviewmodel
                {
                    ModifierId = m.modifierid,
                    ModifierName = m.modifiername
                }).ToList(),
            }).OrderBy(u=>u.ItemName).ToList()

        };

        return Model;
        }
        catch (Exception e)
        {
            
            throw;
        }
      
    }
    public async Task<bool> UpdateQuantityAsync(int orderId, string status, int itemId,int OrderItemId , int quantity)
    {
        var result = await _kotRepository.UpdateQuantityAsync(orderId, status, itemId, OrderItemId , quantity);
        return result;
    }
}
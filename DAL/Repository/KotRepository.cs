using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Pizzashop.DAL.ViewModels;

namespace DAL.Repository;

public class KotRepository : IKotRepository
{
    private readonly PizzaShopContext _db;

    public KotRepository(PizzaShopContext db)
    {
        _db = db;
    }

    public async Task<List<MenuCategory>> GetCategories()
    {
        var categories = await _db.MenuCategories.Where(u => u.IsDeleted == false).OrderBy(u => u.Categoryid).ToListAsync();
        return categories;
    }
    public async Task<Kotviewmodel> GetKotDataAsync(string status,int categoryId)
    {

        var categories = await _db.MenuCategories.Where(u => u.IsDeleted == false).OrderBy(u => u.Categoryid).ToListAsync();

        var orders = await _db.Orders.Include(u => u.OrderTables).ThenInclude(u => u.Table).ThenInclude(u => u.Section)
                                    .Include(u => u.OrderItems).ThenInclude(u => u.Item)
                                    .Include(u => u.OrderItems).ThenInclude(u => u.OrderItemModifiers).ThenInclude(u => u.Modifier)
                                    .Select(o => new Kotviewmodel.OrderDetailsViewModel
                                    {
                                        orderId = o.Orderid,
                                        OrderNo = o.OrderNo,
                                        OrderDate = o.CreatedDate.GetValueOrDefault(),
                                        OrderStatus = status,
                                        TableNo = o.OrderTables.Where(t => t.OrderId == o.Orderid).Select(t=>t.Table).ToList(),
                                        SectionName = o.OrderTables.FirstOrDefault().Table.Section.SectionName,
                                        Items = o.OrderItems
                                        .Where(oi=> status == "In Progress" &&  oi.Quantity-oi.ReadyItem > 0  ||  status == "Ready" && oi.ReadyItem > 0)
                                        .Select(i => new Kotviewmodel.OrderItemViewModel
                                        {
                                            ItemName = i.Item.Itemname,
                                            Quantity = (int)i.Quantity,
                                            PendigQuantity = (int)i.Quantity - (int)i.ReadyItem,
                                            ReadyQuantity = (int)i.ReadyItem,
                                            CategoryId = (int)i.Item.Categoryid,
                                            Modifiers = i.OrderItemModifiers.Select(m => new Kotviewmodel.ModifierViewModel
                                            {
                                                ModifierName = m.Modifier.Modifiername
                                            }).ToList()
                                        }).ToList()
                                    }).ToListAsync();

        

        if(categoryId != 0 )
        {
            orders = orders.Where(o => o.Items.Any(i => i.CategoryId == categoryId)).ToList();
        }

        var kotViewModel = new Kotviewmodel
        {
            Categories = categories,
            OrderDetails = orders
        };

        return kotViewModel;
    }

    public async Task<OrderCardviewmodel> GetKotDetailsAsync(int id,string status)
    {
        var orderDetails = await _db.Orders.Include(u => u.OrderItems).ThenInclude(u => u.Item)
            .Include(u => u.OrderItems).ThenInclude(u => u.OrderItemModifiers).ThenInclude(u => u.Modifier)
            .Where(u => u.Orderid == id)
            .Select(o => new OrderCardviewmodel
            {
                OrderID = o.Orderid,
                OrderNo = o.OrderNo, 
                OrderStatus = status,
                ItemList = o.OrderItems.Select(i => new OrderCardviewmodel.OrderItemsviewmodel
                {
                    ItemId = (int)i.ItemId,
                    ItemName = i.Item.Itemname,
                    IsSelected = true,
                    TotalQuantity = (int)i.Quantity,
                    ReadyQuantity = (int)i.ReadyItem,
                    PendigQuantity = (int)i.Quantity -(int) i.ReadyItem,
                    ModifierList = i.OrderItemModifiers.Select(m => new OrderCardviewmodel.OrderItemModifierviewmodel
                    {
                        ModifierId =(int) m.ModifierId,
                        ModifierName = m.Modifier.Modifiername
                    }).ToList()
                }).ToList()
    
            }).FirstOrDefaultAsync();
    
        return orderDetails;
    }

    public async Task<bool> UpdateQuantityAsync(int orderId,string status,int itemId, int quantity)
    {
        var orderItem = await _db.OrderItems.FirstOrDefaultAsync(u=>u.ItemId == itemId && u.OrderId == orderId);

        if(orderItem == null)
        {
            return false;
        }

        if(status == "In Progress")
        {
            orderItem.ReadyItem = orderItem.ReadyItem + quantity;
        }
        else if(status == "Ready")
        {
            orderItem.ReadyItem = orderItem.ReadyItem - quantity;
        }
      
        _db.OrderItems.Update(orderItem);

         await _db.SaveChangesAsync();
        return true;
    }

}



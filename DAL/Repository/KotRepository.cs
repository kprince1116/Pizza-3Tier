
using System.Data;
using DAL.Interfaces;
using DAL.Models;
using Dapper;
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

    public async Task<List<MenuCategory>> GetCategoriesFromFunctionAsync()
    {
        try
        {
            var categories = await _db.MenuCategories
                .FromSqlRaw("SELECT * FROM GetCategories()")
                .ToListAsync();

            return categories;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<Kotviewmodel> GetKotDataAsync(string status, int categoryId)
    {
        try
        {


            var categories = await _db.MenuCategories.Where(u => u.IsDeleted == false).OrderBy(u => u.Categoryid).AsNoTracking().ToListAsync();
            var orders = await _db.Orders.Include(u => u.OrderTables).ThenInclude(u => u.Table).ThenInclude(u => u.Section)
                                        .Include(u => u.OrderItems).ThenInclude(u => u.Item)
                                        .Include(u => u.OrderItems).ThenInclude(u => u.OrderItemModifiers).ThenInclude(u => u.Modifier)
                                        .Where(u => u.StatusNavigation.Status == "In Progress" || u.StatusNavigation.Status == "Pending")
                                        .Select(o => new Kotviewmodel.OrderDetailsViewModel
                                        {
                                            orderId = o.Orderid,
                                            OrderNo = o.OrderNo,
                                            OrderDate = o.CreatedDate.GetValueOrDefault(),
                                            OrderStatus = status,
                                            OrderInstruction = o.Instruction,
                                            TableNo = o.OrderTables.Where(t => t.OrderId == o.Orderid).Select(t => t.Table).ToList(),
                                            SectionName = o.OrderTables.FirstOrDefault().Table.Section.SectionName,
                                            Items = o.OrderItems
                                            .Where(oi => status == "In Progress" && oi.Quantity - oi.ReadyItem > 0 || status == "Ready" && oi.ReadyItem > 0)
                                            .Select(i => new Kotviewmodel.OrderItemViewModel
                                            {
                                                ItemName = i.Item.Itemname,
                                                Quantity = (int)i.Quantity,
                                                PendigQuantity = (int)i.Quantity - (int)i.ReadyItem,
                                                ReadyQuantity = (int)i.ReadyItem,
                                                ItemInstrucion = i.OrderItemInstruction,
                                                CategoryId = (int)i.Item.Categoryid,
                                                Modifiers = i.OrderItemModifiers.Select(m => new Kotviewmodel.ModifierViewModel
                                                {
                                                    ModifierName = m.Modifier.Modifiername
                                                }).ToList()
                                            }).ToList()
                                        }).ToListAsync();



            if (categoryId != 0)
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
        catch (Exception e)
        {

            throw;
        }
    }

    public async Task<List<OrderDtoViewmodel>> GetKotData(string status, int categoryId)
    {
        try
        {
            using var conn = _db.Database.GetDbConnection();
            await conn.OpenAsync();
            var orders = await conn.QueryAsync<OrderDtoViewmodel>(
            "SELECT * FROM GetOrders(@Status)",
            new { Status = status },
            commandType: CommandType.Text
            );

            if (categoryId != 0)
            {
                orders = orders.Where(o => o.CategoryId == categoryId).ToList();
            }

            return orders.ToList();

        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<OrderCardviewmodel> GetKotDetailsAsync(int id, string status)
    {
        var orderDetails = await _db.Orders.Include(u => u.OrderItems).ThenInclude(u => u.Item)
            .Include(u => u.OrderItems).ThenInclude(u => u.OrderItemModifiers).ThenInclude(u => u.Modifier)
            .Where(u => u.Orderid == id)
            .Select(o => new OrderCardviewmodel
            {
                OrderID = o.Orderid,
                OrderNo = o.OrderNo,
                OrderStatus = status,
                ItemList = o.OrderItems.Where(oi => status == "In Progress" && oi.Quantity - oi.ReadyItem > 0 || status == "Ready" && oi.ReadyItem > 0).Select(i => new OrderCardviewmodel.OrderItemsviewmodel
                {
                    ItemId = (int)i.ItemId,
                    ItemName = i.Item.Itemname,
                    IsSelected = true,
                    TotalQuantity = (int)i.Quantity,
                    ReadyQuantity = (int)i.ReadyItem,
                    PendigQuantity = (int)i.Quantity - (int)i.ReadyItem,
                    ModifierList = i.OrderItemModifiers.Select(m => new OrderCardviewmodel.OrderItemModifierviewmodel
                    {
                        ModifierId = (int)m.ModifierId,
                        ModifierName = m.Modifier.Modifiername
                    }).ToList()
                }).ToList()

            }).FirstOrDefaultAsync();

        return orderDetails;
    }

    public async Task<List<KotOrderCardDTOViewModel>> GetKotCardData(int id, string status, int CategoryId )
    {
        try
        {
            using var conn = _db.Database.GetDbConnection();
            await conn.OpenAsync();
            var orders = await conn.QueryAsync<KotOrderCardDTOViewModel>(
            "SELECT * FROM GetKotDetails(@p_order_id,@p_status)",
            new { p_status = status, p_order_id = id },
            commandType: CommandType.Text
            );

            if (CategoryId != 0)
            {
                orders = orders.Where(o => o.categoryid == CategoryId).ToList();
            }

            return orders.ToList();
        }
        catch (Exception e)
        {
            throw;
        }
    }
    public async Task<bool> UpdateQuantityAsync(int orderId, string status, int itemId,int OrderItemId , int quantity)
    {
        var orderItem = await _db.OrderItems.FirstOrDefaultAsync(u => u.ItemId == itemId && u.OrderId == orderId & u.Id == OrderItemId);

        if (orderItem == null)
        {
            return false;
        }

        if (status == "In Progress")
        {
            orderItem.ReadyItem = orderItem.ReadyItem + quantity;
            if (orderItem.ReadyItem == orderItem.Quantity)
            {
                orderItem.Status = "Ready";
            }
        }
        else if (status == "Ready")
        {
            orderItem.ReadyItem = orderItem.ReadyItem - quantity;
            if (orderItem.ReadyItem != orderItem.Quantity)
            {
                orderItem.Status = "In Progress";
            }

        }

        _db.OrderItems.Update(orderItem);

        await _db.SaveChangesAsync();
        return true;
    }
    // public async Task<bool> UpdateQuantityAsync(int orderId, string status, int itemId,int OrderItemId , int quantity)
    // {
    //     try
    //     {
    //         using var conn = _db.Database.GetDbConnection();
    //         await conn.OpenAsync();

    //         await conn.ExecuteAsync(
    //             "Call UpdateQuantity(@p_order_id,@p_status,@p_item_id,@p_quantity,p_order_item_id)",
    //             new
    //             {
    //                 p_order_id = orderId,
    //                 p_status = status,
    //                 p_item_id = itemId,
                      // p_order_item_id = OrderItemId,
    //                 p_quantity = quantity
    //             }
    //         );

    //         return true;
    //     }
    //     catch (Exception e)
    //     {
    //         return false;
    //     }

    // }

}

using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Pizzashop.DAL.ViewModels;

namespace DAL.Repository;

public class OrderAppMenuRepository : IOrderAppMenuRepository
{
    private readonly PizzaShopContext _db;

    public OrderAppMenuRepository(PizzaShopContext db)
    {
        _db = db;
    }

    public async Task<List<MenuCategory>> GetCategories()
    {
        // var categories = await _db.MenuCategories.Where(u=>u.IsDeleted == false).OrderBy(u=>u.Name).ToListAsync();
        // return categories;
          var categories = await _db.MenuCategories
                .FromSqlRaw("SELECT * FROM GetCategories()")
                .ToListAsync();
          return categories;
    }

     public async Task<List<MenuItem>> GetItems(int CategoryId,string SearchKey)
     {
        // var items = await _db.MenuItems.Where(u=>u.IsDeleted == false).OrderBy(u=>u.Itemname).ToListAsync();
        var items = await _db.MenuItems
                .FromSqlRaw("SELECT * FROM GetItems()")
                .ToListAsync();

        if(CategoryId != 0)
        {
            items = items.Where(u=>u.Categoryid == CategoryId).ToList();
        }

        if(!string.IsNullOrEmpty(SearchKey))
        {
            var lowerSearchQuery = SearchKey.ToLower();

            items = items.Where(u=>u.Itemname.ToLower().Contains(lowerSearchQuery) || u.Itemtype.ToLower().Contains(lowerSearchQuery) || u.Rate.ToString().Contains(lowerSearchQuery)).ToList();
        }


        return items;
     }

    public async Task<List<MenuItem>> GetFavouriteItems(string SearchKey)
    {
        // var items = await _db.MenuItems.Where(u=>u.IsDeleted == false && u.IsFavourite == true).OrderBy(u=>u.Itemname).ToListAsync();
         var items = await _db.MenuItems
                .FromSqlRaw("SELECT * FROM GetFavouriteItems()")
                .ToListAsync();

        if(!string.IsNullOrEmpty(SearchKey))
        {
            var lowerSearchQuery = SearchKey.ToLower();

            items = items.Where(u=>u.Itemname.ToLower().Contains(lowerSearchQuery) || u.Itemtype.ToLower().Contains(lowerSearchQuery) || u.Rate.ToString().Contains(lowerSearchQuery)).ToList();
        }

        return items;
    }

    public async Task<MenuItem> GetItemById(int ItemId)
    {
        var item = await _db.MenuItems.FirstOrDefaultAsync(u=>u.Itemid == ItemId && u.IsDeleted == false);
        return item;
    }

    public async Task UpdateItem(MenuItem item)
    {
        _db.MenuItems.Update(item);
        await _db.SaveChangesAsync();
    }

    public async Task<MenuItem> GetItemForModalById(int ItemId)
    {
        var item = await _db.MenuItems.Include(u=>u.Itemmodifiergroups.Where(v=>v.IsDeleted == false))
                                        .ThenInclude(u=>u.Modifiergroup)
                                        .ThenInclude(u=>u.Modifiermappings.Where(V=>V.IsDeleted == false))
                                        .ThenInclude(u=>u.Modifier)
                                        .FirstOrDefaultAsync(u=>u.Itemid == ItemId && u.IsDeleted == false);
        return item;
    }

    public async Task<Order> GetTableData( int OrderId)
    {
        var table = await _db.Orders.Include(u=>u.OrderTables)
                                    .ThenInclude(u=>u.Table).
                                    ThenInclude(u=>u.Section)
                                    .Include(u=>u.OrderItems)
                                    .Include(u=>u.StatusNavigation).Include(u=>u.PaymentModeNavigation).Where(u=>u.Orderid == OrderId).FirstOrDefaultAsync();
                                   
        return table;
    }

    public async Task<List<OrderItem>> GetOrderItems(int OrderId)
    {
        var orderItems = await _db.OrderItems.Include(u=>u.Item)
                                            .Include(u=>u.OrderItemModifiers).ThenInclude(u=>u.Modifier)
                                             .Where(u=>u.OrderId == OrderId).Where(u=>u.IsDeleted == false).ToListAsync();
        return orderItems;  
    }

    public async Task<Order> GetCustomerDetails(int OrderId)
    {
        var customerDetails = await _db.Orders.Include(u=>u.Customer)
                                              .FirstOrDefaultAsync(u=>u.Orderid == OrderId);
        return customerDetails;
    }

    public async Task<Customer> GetCustomerById(int CustomerId)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(u=>u.Customerid == CustomerId);
        return customer;
    }

    public async Task<Order> GetOrderDetails(int OrderId)
    {
        var order = await _db.Orders.Include(u=>u.OrderTables).Include(u=>u.StatusNavigation).Include(u=>u.PaymentModeNavigation).FirstOrDefaultAsync(u=>u.Orderid == OrderId);
        return order;
    }
    public async Task UpdateCustomer(Customer customer)
    {
        _db.Customers.Update(customer);
        await _db.SaveChangesAsync();
    }
    public async Task UpdateOrder(Order order)
    {
         _db.Orders.Update(order);
        await _db.SaveChangesAsync();
    }

    public async Task<Order> GetOrderComments(int OrderId)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(u=>u.Orderid == OrderId);
        return order;
    }
    public async Task<OrderItem> GetItemComments(int OrderId)
    {
        var order = await _db.OrderItems.FirstOrDefaultAsync(u=>u.Id == OrderId);
        return order;
    }

    public async Task<List<Taxesandfess>> GetTaxList()
    {
        var taxes = await _db.Taxesandfesses.Where(u=>u.Isactive == true && u.Isdeleted == false).OrderBy(u=>u.Taxname).ToListAsync();
        return taxes;
    }

    public async Task<List<OrderTax>> GetTaxLists(int OrderId)
    {
        var taxes = await _db.OrderTaxes.Include(u=>u.Tax).Where(u=>u.OrderId==OrderId).ToListAsync();
        return taxes;
    }

    public async Task<Table> GetTable(int TableId)
    {
       return await _db.Tables.FirstOrDefaultAsync(u=>u.Tableid == TableId);
    }

    public async Task<OrderItem> GetorderItemForDelete(int deleteId)
    {
        return await _db.OrderItems.FirstOrDefaultAsync(u=>u.Id == deleteId);
    }

    public async Task<OrderItem> GetOrderItem(int OrderItemId)
    {
        return await _db.OrderItems.Include(u=>u.OrderItemModifiers).FirstOrDefaultAsync(u=>u.Id == OrderItemId);
    }

    public async Task UpdateOrderItem(OrderItem orderItem)
    {
        _db.OrderItems.Update(orderItem);
        await _db.SaveChangesAsync();
    }
    
    public async Task<OrderItem> AddOrderItem(OrderItem orderItem)
    {
        try
        {
         _db.OrderItems.Update(orderItem);
        await _db.SaveChangesAsync();
        return orderItem;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       
    }

    public async Task<OrderItemModifier> AddOrderItemModifier(OrderItemModifier orderItemModifier)
    {
        try
        {
            _db.OrderItemModifiers.Update(orderItemModifier);
        await _db.SaveChangesAsync();
        return orderItemModifier;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
        
    }

    public async Task AddTax( OrderTax orderTax)
    {
        _db.OrderTaxes.Update(orderTax);
        await _db.SaveChangesAsync();
    }

    public async Task<Paymentmode> GetPayment(int PaymentModeId)
    {
        return await _db.Paymentmodes.FirstOrDefaultAsync(u => u.Id == PaymentModeId);
    }

    public async Task UpdateOrderPayment(Paymentmode orderPayemnt)
    {
        try
        {
            _db.Paymentmodes.Update(orderPayemnt);
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }

    public async Task<Paymentmode> AddPayment(Paymentmode payment)
    {
        _db.Paymentmodes.Update(payment);
        await _db.SaveChangesAsync();
        return payment;
    }

    public async Task<List<OrderTable>> GetTables(int Orderid)
    {
        return await _db.OrderTables.Where(u=>u.OrderId == Orderid && u.IsDeleted == false).ToListAsync();
    }

    public async Task<Table> ChangeTableData(int CustomerId)
    {
        return await _db.Tables.FirstOrDefaultAsync(u=>u.Tableid == CustomerId);
    }

    public async Task SaveTableData(Table table)
    {
        _db.Tables.Update(table);
        await _db.SaveChangesAsync();
    }

    public async Task updateOrderTable(OrderTable table)
    {
        _db.OrderTables.Update(table);
        await _db.SaveChangesAsync();
    }

    public async Task updateTable(Table tables)
    {
         _db.Tables.Update(tables);
        await _db.SaveChangesAsync();
    }

    public async Task<Paymentmode> GetPaymentDetails(int PaymentId)
    {
        return await _db.Paymentmodes.FirstOrDefaultAsync(u=>u.Id == PaymentId);
    }

    public async Task<bool> CheckReadyQuantity(int orderId)
    {
        var order = await _db.Orders.Include(u=>u.OrderItems.Where(u=>u.IsDeleted == false)).FirstOrDefaultAsync(U=>U.Orderid == orderId && U.Isdelete == false);

        if(order == null)
        {
            return false;
        }

        if(order.OrderItems.All(u=>u.ReadyItem == u.Quantity))
        {
            return true;
        }
        else{
            return false;
        }

    }
    public async Task<bool> CheckReadyQuantityForCancel(int orderId)
    {
        var order = await _db.Orders.Include(u=>u.OrderItems.Where(u=>u.IsDeleted == false)).FirstOrDefaultAsync(U=>U.Orderid == orderId && U.Isdelete == false);

        if(order == null)
        {
            return false;
        }

        if(order.OrderItems.All(u=>u.ReadyItem ==0 ))
        {
            return true;
        }
        else{
            return false;
        }

    }

    public async Task<Order> GetOrderDetailsForRating(int OrderId)
    {
        return await _db.Orders.Include(u=>u.RatingNavigation).FirstOrDefaultAsync(u=>u.Orderid == OrderId);
    }

    public async Task<Rating> AddCustomerRatting(Rating rating)
    {
        _db.Ratings.Add(rating);
        await _db.SaveChangesAsync();
        return rating;
    }
   
   
}

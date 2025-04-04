using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

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
        return await _db.MenuCategories.Where(u=>u.IsDeleted==false).OrderBy(u=>u.Categoryid).ToListAsync();
    }

    public async Task<List<Order>> GetOrders()
    {
        return await _db.Orders.Where(u=>u.Isdelete==false).ToListAsync();
    }

}

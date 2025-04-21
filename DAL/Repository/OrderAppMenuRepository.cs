using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

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
        var categories = await _db.MenuCategories.Where(u=>u.IsDeleted == false).OrderBy(u=>u.Name).ToListAsync();
        return categories;
    }

     public async Task<List<MenuItem>> GetItems(int CategoryId,string SearchKey)
     {
        var items = await _db.MenuItems.Where(u=>u.IsDeleted == false).OrderBy(u=>u.Itemname).ToListAsync();

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
        var items = await _db.MenuItems.Where(u=>u.IsDeleted == false && u.IsFavourite == true).OrderBy(u=>u.Itemname).ToListAsync();

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
}

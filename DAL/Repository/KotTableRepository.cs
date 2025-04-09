using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Pizzashop.DAL.ViewModels;
using static System.Collections.Specialized.BitVector32;

namespace DAL.Repository;

public class KotTableRepository : IKotTableRepository
{
    private readonly PizzaShopContext _db;

    public KotTableRepository(PizzaShopContext db)
    {
        _db = db;
    }

    public async Task<List<Models.Section>> GetSections()
    {
        return await _db.Sections.Where(u => u.Isdeleted == false) .OrderBy(u => u.Sectionid).ToListAsync();
    }

    public async Task<List<Table>> GetTablesBySectionIdAsync(int sectionId)
    {
        return await _db.Tables .Where(t => t.Sectionid == sectionId && t.Isdeleted == false).OrderBy(u=>u.Tableid).ToListAsync();
    }

   
}

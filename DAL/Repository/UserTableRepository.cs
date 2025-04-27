using DAL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.EntityFrameworkCore;
using Pizzashop.DAL.ViewModels;

namespace DAL.Repository;

public class UserTableRepository : IUserTableRepository
{

    private readonly PizzaShopContext _db;

    public UserTableRepository(PizzaShopContext db)
    {
        _db = db;
    }
    public List<Section> GetSections()
    {
        return _db.Sections.Where(u=> u.Isdeleted == false).OrderBy(u=>u.Sectionid).ToList();
    }


    //CRUD SECTIONS

    public async Task<bool> AddSection(Tablesviewmodel model)
    {
        bool exists = await _db.Sections.AnyAsync(u=>u.SectionName.ToLower()==model.section.SectionName.ToLower());
        
        if(exists)
        {
            return false;
        }

        var section = new Section
        {
            SectionName = model.section.SectionName,
            Description = model.section.Description
        };
        _db.Sections.Add(section);
        bool success = await _db.SaveChangesAsync() > 0;
        return success;
    }

    public async Task<bool> GetSectionName(string SectionName)
    {
        var exists = await _db.Sections.AnyAsync(u=>u.SectionName.ToLower() == SectionName.ToLower());

        if(exists)
        {
            return false;
        }
        return true;
    }
    public Task<Section> GetSectionByIdForEdit(int Sectionid)
    {
        return _db.Sections.FirstOrDefaultAsync(u=>u.Sectionid == Sectionid);
    }

    public async Task UpdateSectionForEditAsync(Section existingSection)
    {
        _db.Sections.Update(existingSection);
        await _db.SaveChangesAsync();
    }

    public async Task<Section> GetSectionByIdForDelte(int id)
    {
        return await _db.Sections.FirstOrDefaultAsync(u=>u.Sectionid == id);
    }

    
    public async Task UpdateSectionAsync(Section existingSection)
    {
        _db.Sections.Update(existingSection);
         await _db.SaveChangesAsync() ;
    }

   

     public async Task<Tableviewmodel> GetTablesBySection(int id,int pageNo,int pageSize,string searchKey)
     {
        var tables = _db.Tables.Where( u=>u.Sectionid == id && u.Isdeleted == false && u.TableName.ToLower().Contains(searchKey.ToLower())).Select(
            u=> new TableItemviewmodel
            {
                Tableid = u.Tableid,
                TableName = u.TableName,
                Capacity = u.Capacity,
                Isavailable = u.Isavailable,
                Sectionid = u.Sectionid,
            }
        ).OrderBy(u=>u.Tableid);

        var totalRecords = tables.Count();

        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        List<TableItemviewmodel> tablesList = tables.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

        return new Tableviewmodel
        {
            TableItems = tablesList,
            CurrentPage = pageNo,
            TotalPages  = totalPages,
            PageSize = pageSize ,
            TotalRecords = totalRecords,
            Sectionid = id
        };

     }

      public async Task AddTable(Tablesviewmodel model)
    {
        var table = new Table
        {
            Sectionid = model.AddTable.Sectionid,
            Tableid = model.AddTable.Tableid,
            TableName = model.AddTable.TableName,
            Capacity = model.AddTable.Capacity,
            Isavailable = model.AddTable.Isavailable,
        };

        _db.Tables.Update(table);
        await _db.SaveChangesAsync();
    }
    
    public async Task<Table> GetTableByIdForDelte( int id)
    {
        return await _db.Tables.FirstOrDefaultAsync(u=>u.Tableid == id);
    }

     public async Task UpdateTableAsync(Table existingTable)
    {
        _db.Tables.Update(existingTable);
        await _db.SaveChangesAsync();
    }

    public async Task<EditTableviewmodel> GetEditTable(int id)
    {
        return await _db.Tables.Where(u=>u.Tableid == id).Select(
            u => new EditTableviewmodel
            {
                Sectionid = u.Sectionid,
                Tableid = u.Tableid,
                TableName = u.TableName,
                Capacity = u.Capacity ,
                sections = _db.Sections.ToList(),
                Isavailable = u.Isavailable
            }
        ).FirstOrDefaultAsync();
    }

     public async Task<Table> GetEditTableId(int id)
     {
        return await _db.Tables.FirstOrDefaultAsync(u => u.Tableid == id);
     } 

    public async Task UpdateTableForEditAsync(Table existingTable)
    {
        _db.Tables.Update(existingTable);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteTableAsync(List<int> tableLists)
    {
        for(int i=0 ; i<tableLists.Count(); i++)
        {
            Table table = _db.Tables.Where(u=>u.Tableid == tableLists[i]).FirstOrDefault();
            table.Isdeleted = true ;
            // _db.Tables.Update(table);
          
        }
          await _db.SaveChangesAsync();
    }

}

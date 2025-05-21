using BAL.Models.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
using Pizzashop.DAL.ViewModels;

namespace BAL.Services;

public class Table : ITable
{

    private readonly IUserTableRepository _userTableRepository;


    public Table(IUserTableRepository userTableRepository)
    {
        _userTableRepository = userTableRepository;
    }
    public List<Sectionviemodel> GetSections()
    {
        var sections = _userTableRepository.GetSections();

        var Sectionviemodels = sections.Select(s => new Sectionviemodel
        {
            Sectionid = s.Sectionid,
            SectionName = s.SectionName,
            Description = s.Description
        }).ToList();
        return Sectionviemodels;
    }


    //CRUD SECTIONS

    public async Task<bool> AddSection(Tablesviewmodel model)
    {
        return await _userTableRepository.AddSection(model);
    }

    public async Task<bool> EditSection(Tablesviewmodel model)
    {
        var existsectionname = await _userTableRepository.GetSectionName(model.section.SectionName);

        if(existsectionname == false)
        {
            return false;
        }

        var section = await _userTableRepository.GetSectionByIdForEdit(model.section.Sectionid);

        if(section == null)
        {
            return false;
        }

        section.SectionName = model.section.SectionName;
        section.Description = model.section.Description;    

        await _userTableRepository.UpdateSectionForEditAsync(section);

        return true;
    }
    public async Task<bool> GetSectionByIdForDelte(int id)
    {
        var existingSection = await _userTableRepository.GetSectionByIdForDelte(id);

        if(existingSection == null)
        {
            return false;
        }

        existingSection.Isdeleted = true;

        await _userTableRepository.UpdateSectionAsync(existingSection);

        return true;

    }

     public async Task<bool> GetTableByIdForDelte(int id)
     {
          var existingTable = await _userTableRepository.GetTableByIdForDelte(id);

            if(existingTable == null)
        {
            return false;
        }

        existingTable.Isdeleted = true;

         await _userTableRepository.UpdateTableAsync(existingTable);

         return true;

     }

    public async Task<Tableviewmodel> GetTablesBySection(int id,int pageNo,int pageSize,string searchKey)
    {
        var tables = await _userTableRepository.GetTablesBySection(id,pageNo,pageSize,searchKey);

        return tables;
    }

    //CRUD TABLE
    public async Task<bool> AddTable(Tablesviewmodel model)
    {
        if(model.AddTable == null)
        {
            return false;
        }

        await _userTableRepository.AddTable(model);

        return true;
    }

    public async Task<EditTableviewmodel> GetEditTable(int id)
    {
        return await _userTableRepository.GetEditTable(id);
    }

    public async Task<bool> EditTable(EditTableviewmodel model)
    {

        var existingTable = await _userTableRepository.GetEditTableId(model.Tableid);

        if(existingTable == null)
        {
            return false;
        }

        existingTable.Sectionid = model.Sectionid;
        existingTable.Tableid = model.Tableid;
        existingTable.TableName = model.TableName;
        existingTable.Capacity = model.Capacity;
        existingTable.Isavailable = model.Isavailable;

        await _userTableRepository.UpdateTableForEditAsync(existingTable);

        return true;

    }

      public async Task  DeleteTableAsync(List<int> tableLists)
      {
        await _userTableRepository.DeleteTableAsync(tableLists);
      }

}

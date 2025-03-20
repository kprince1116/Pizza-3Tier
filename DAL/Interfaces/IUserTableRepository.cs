using DAL.Models;
using DAL.ViewModels;
using Pizzashop.DAL.ViewModels;

namespace DAL.Interfaces;

public interface IUserTableRepository
{
    List<Section> GetSections();

    //CRUD SECTIONS

    public Task<bool> AddSection(Sectionviemodel model);

    public Task<Section> GetSectionByIdForEdit(int Sectionid);

    Task UpdateSectionForEditAsync(Section existingSection);
    public Task<Section> GetSectionByIdForDelte(int id);
    Task UpdateSectionAsync(Section existingSection);

    public Task<Tableviewmodel> GetTablesBySection(int id,int pageNo,int pageSize,string searchKey);

    //CRUD TABLE
    Task AddTable(Tablesviewmodel model);

    public Task<Table> GetTableByIdForDelte( int id);

    Task UpdateTableAsync(Table existingTable);

    public Task<EditTableviewmodel> GetEditTable(int id);
}

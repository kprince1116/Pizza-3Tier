using DAL.ViewModels;
using Pizzashop.DAL.ViewModels;

namespace BAL.Models.Interfaces;

public interface ITable
{
    public List<Sectionviemodel> GetSections();

    //CRUD SECTIONS
    public Task<bool> AddSection(Sectionviemodel model);

    public Task<bool> EditSection(Sectionviemodel model);

    public Task<bool> GetSectionByIdForDelte(int id);

    public Task<Tableviewmodel> GetTablesBySection(int id,int pageNo,int pageSize,string searchKey);

    public Task<bool> AddTable(Tablesviewmodel model);

    public Task<bool> GetTableByIdForDelte(int id);

    public Task<EditTableviewmodel> GetEditTable(int id);

    public Task<bool> EditTable(EditTableviewmodel model);

    public Task  DeleteTableAsync(List<int> tableLists);
}

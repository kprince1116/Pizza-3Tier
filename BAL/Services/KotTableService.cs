using BAL.Models.Interfaces;
using DAL.Interfaces;
using Pizzashop.DAL.ViewModels;
namespace BAL.Services;
using DAL.Enum;
using DAL.Models;
using static DAL.Enum.Enum;

public class KotTableService : IKotTableService
{
    private readonly IKotTableRepository _kotTableRepository;

    public KotTableService(IKotTableRepository kotTableRepository)
    {
        _kotTableRepository = kotTableRepository;
    }
    public async Task<List<KotTableviewmodel>> GetSections()
    {
        var sections = await _kotTableRepository.GetSections();

        var Table = new List<KotTableviewmodel>();

         foreach (var section in sections)
    {
        var tables = await _kotTableRepository.GetTablesBySectionIdAsync(section.Sectionid);
     
        var kotTableViewModel = new KotTableviewmodel
        {
            SectionId = section.Sectionid,
            SectionName = section.SectionName,
            AvailableCount = tables.Count(t=>t.Status == "Available"),
            RunningCount = tables.Count(t=>t.Status == "Running"),
            AssignedCount = tables.Count(t=>t.Status == "Assigned"),
            Tables = tables.Select(t => new OrderAppTable
            {
                TableId = t.Tableid,
                Name = t.TableName,
                Capacity = t.Capacity, 
                Status = t.Status,
                AssignTime = t.CreatedDate ?? DateTime.Now,
            }).ToList()
        };

        Table.Add(kotTableViewModel);
    }


        return Table;
    }

    public async Task<bool> AddWaitingToken(waitingtokenviewmodel model)
     {
        if(model == null)
        {
            return false;
        }
        await _kotTableRepository.AddWaitingToken(model);
        return true;
     }

    public async Task<List<Section>> GetSectionList()
    {
        var sections = await _kotTableRepository.GetSectionList();
        return sections;
    }

    public async Task<List<waitingtokenviewmodel>> GetWaitingList()
    {
        var waitingList = await _kotTableRepository.GetWaitingList();
        return waitingList;
    }
    
}

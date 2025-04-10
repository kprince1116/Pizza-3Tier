using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace BAL.Models.Interfaces;

public interface IKotTableService
{
    Task<List<KotTableviewmodel>> GetSections();

    Task<bool> AddWaitingToken(waitingtokenviewmodel model);

    Task<List<Section>> GetSectionList();

    Task<List<waitingtokenviewmodel>> GetWaitingList();
}

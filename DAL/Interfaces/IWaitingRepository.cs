using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace DAL.Interfaces;

public interface IWaitingRepository
{
    Task<List<Section>> GetSections();

    Task<List<waitingtokenviewmodel>> GetWaitingList(int sectionId);

    Task<waitingtokenviewmodel> EditToken(int id);

    Task<WaitingToken> GetTokenId(int id);

    Task Update(WaitingToken waitingToken);

}

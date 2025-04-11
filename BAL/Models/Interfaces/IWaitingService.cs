using System.Security.Principal;
using Pizzashop.DAL.ViewModels;

namespace BAL.Models.Interfaces;

public interface IWaitingService
{
    public Task<waitingviemodel> GetSections();

    public Task<waitingviemodel> GetWaitingList(int sectionId);

    public Task<waitingtokenviewmodel> EditToken(int id);

    public Task<bool> UpdateWaitingToken(waitingtokenviewmodel model);

    public Task<bool> DeleteToken(int id);
}

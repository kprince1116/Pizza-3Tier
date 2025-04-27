using System.Security.Principal;
using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace BAL.Models.Interfaces;

public interface IWaitingService
{
    public Task<waitingviemodel> GetSections();

    public Task<waitingviemodel> GetWaitingList(int sectionId);

    public Task<waitingtokenviewmodel> GetCustomerDetailsByEmail(int sectionid , string email);

    public Task<waitingtokenviewmodel> EditToken(int id);

    public Task<bool> UpdateWaitingToken(waitingtokenviewmodel model);

    public Task<bool> DeleteToken(int id);

    Task<waitingviemodel> GetTableDetails();

    public Task<int> AssignTable(waitingtokenviewmodel model);
}

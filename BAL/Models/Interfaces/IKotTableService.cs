using Pizzashop.DAL.ViewModels;

namespace BAL.Models.Interfaces;

public interface IKotTableService
{
    Task<List<KotTableviewmodel>> GetSections();
}

using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace DAL.Interfaces;

public interface IKotTableRepository
{
     Task<List<Section>> GetSections();

    Task<List<Table>> GetTablesBySectionIdAsync(int sectionId);
}

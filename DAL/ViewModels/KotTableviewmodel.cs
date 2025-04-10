using DAL.Models;
using DAL.ViewModels;

namespace Pizzashop.DAL.ViewModels;

public class KotTableviewmodel
{
    public int SectionId { get; set; }
    public string SectionName { get; set; } = null!;
    public int AvailableCount { get; set; }
    public int RunningCount { get; set; }
    public int AssignedCount { get; set; }
    public List<OrderAppTable> Tables { get; set; } = new List<OrderAppTable>();

}
public class OrderAppTable{
    public int TableId { get; set; }
    public string Name { get; set; } = null!;

    public decimal Amount {get;set;} = 0;
    public int Capacity { get; set; }
    public string Status { get; set; } 
    public DateTime? AssignTime { get; set; }
}
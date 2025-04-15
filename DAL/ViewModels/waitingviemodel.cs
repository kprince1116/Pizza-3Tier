using DAL.Models;

namespace Pizzashop.DAL.ViewModels;

public class waitingviemodel
{
    public List<waitingtokenviewmodel> waiting { get; set; }
    public List<Section> sections { get; set; } 
    public List<WaitingTable> AvailableTables { get; set; } = new List<WaitingTable>();
    public int TotalWaiting { get; set; }
    public int sectionId { get; set; }
    public int TableId { get; set; }
    public int Id { get; set; }
    public int customerId { get; set; }
    
}

public class WaitingTable{
    public int sectionId { get; set; }
    public int TableId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Amount {get;set;} = 0;
    public int Capacity { get; set; }
    public string Status { get; set; } 
    public DateTime? AssignTime { get; set; }
 }

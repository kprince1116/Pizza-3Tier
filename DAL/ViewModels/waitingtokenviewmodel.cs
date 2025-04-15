using DAL.Models;

namespace Pizzashop.DAL.ViewModels;

public class waitingtokenviewmodel
{
    public int Id { get; set; }
    public int customerId { get; set; }
    public string Email {get; set;}

    public string Name { get; set; }

    public string Phone { get; set; }

    public int NoOfPerson { get; set; }

    public DateTime CreatedAt { get; set; }

    public int tableId { get; set; }

    public int sectionId { get; set; }

    public string sectionName { get; set; }

    public List<Section> sections { get; set; }

}

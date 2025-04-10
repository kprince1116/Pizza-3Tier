using DAL.Models;

namespace Pizzashop.DAL.ViewModels;

public class waitingtokenviewmodel
{
    public int Id { get; set; }
    public string Email {get; set;}

    public string Name { get; set; }

    public string Phone { get; set; }

    public int NoOfPerson { get; set; }

    public int sectionId { get; set; }

    public List<Section> sections { get; set; }

}

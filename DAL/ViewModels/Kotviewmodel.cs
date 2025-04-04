using DAL.Models;

namespace Pizzashop.DAL.ViewModels;

public class Kotviewmodel
{
    public List<MenuCategory> Categories { get; set; }

    public List<cardviewmodel> Card {get; set; }

}

using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace Pizzashop.DAL.ViewModels;

public class waitingviemodel
{
    public List<waitingtokenviewmodel> waiting { get; set; }
    public List<Section> sections { get; set; }
}

using DAL.ViewModels;

namespace Pizzashop.DAL.ViewModels;

public class Tablesviewmodel
{
    public IEnumerable<Sectionviemodel> Sections { get; set; }
    public Tableviewmodel Tables {get; set;}
   public AddTableviewmodel AddTable {get; set;}

   public Sectionviemodel section {get; set;}

}

using DAL.Models;

namespace Pizzashop.DAL.ViewModels;

public class AddTableviewmodel
{
   
     public int Tableid { get; set; }

    public string TableName { get; set; } = null!;

    public int? Sectionid { get; set; }

    public int Capacity { get; set; }

    public bool? Isavailable { get; set; }

    public virtual Section? Sections { get; set; }

   
}

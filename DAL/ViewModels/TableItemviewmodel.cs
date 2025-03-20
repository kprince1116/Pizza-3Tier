namespace DAL.ViewModels;

public class TableItemviewmodel
{
    public int Tableid { get; set; }

    public string TableName { get; set; } = null!;

    public int? Sectionid { get; set; }

    public int Capacity { get; set; }
    
    public bool? Isavailable { get; set; }

    public bool? Isdeleted { get; set; }

   
}

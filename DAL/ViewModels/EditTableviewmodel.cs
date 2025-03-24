using System.ComponentModel.DataAnnotations;
using DAL.Models;
using DAL.ViewModels;
// using static System.Collections.Specialized.BitVector32;

namespace  Pizzashop.DAL.ViewModels;

public class EditTableviewmodel
{
    public IEnumerable<Section> sections { get; set; }

    public int Tableid { get; set; }

    [Required(ErrorMessage ="TableName is Required")]
    public string TableName { get; set; } = null!;

    public int? Sectionid { get; set; }

    [Required(ErrorMessage ="Capacity is Required")]
    public int Capacity { get; set; }

    public bool? Isavailable { get; set; }

    public virtual Section? Sections { get; set; }

}

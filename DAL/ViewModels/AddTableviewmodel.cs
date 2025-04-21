using System.ComponentModel.DataAnnotations;
using DAL.Models;

namespace Pizzashop.DAL.ViewModels;

public class AddTableviewmodel
{
   
    public int Tableid { get; set; }

    [Required(ErrorMessage ="TableName is Required")]
    public string TableName { get; set; } = null!;

    public int? Sectionid { get; set; }

    [Required(ErrorMessage ="Capacity is Required")]
    public int Capacity { get; set; }

    public bool? Isavailable { get; set; }

    public string status { get; set; } 

    public virtual Section? Sections { get; set; }

   
}

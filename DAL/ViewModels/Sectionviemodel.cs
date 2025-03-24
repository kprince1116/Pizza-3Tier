using System.ComponentModel.DataAnnotations;
using DAL.Models;

namespace DAL.ViewModels;

public class Sectionviemodel
{
     public int Sectionid { get; set; }

    [Required(ErrorMessage ="SectionName is Required")]
    public string SectionName { get; set; } = null!;

    [Required(ErrorMessage ="Description is Required")]
    public string Description { get; set; } = null!;

}

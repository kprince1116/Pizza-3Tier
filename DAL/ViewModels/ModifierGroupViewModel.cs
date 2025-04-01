using System.ComponentModel.DataAnnotations;
using Pizzashop.DAL.ViewModels;

namespace Pizzashop.DAL.ViewModels;

public class ModifierGroupViewModel
{
    public int ModifierId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } 
    public string? Description { get; set; }
    public List<ModifierItemViewModel> ModifierItemList { get; set; } = new List<ModifierItemViewModel>();
}

using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace  Pizzashop.DAL.ViewModels;

public class menuviewmodel
{
        public IEnumerable<Categoryviewmodel> Categories { get; set; }
        public IEnumerable<ModifierGroupViewModel> Modifiers  { get; set; }
        public IEnumerable<ModifierItemViewModel> ModifierItems {get; set;}
        public AddItemviewmodel Additem {get; set;}
        public EditItemviewmodel EditItem {get; set;}
        public IEnumerable<Unit> units {get; set;} 
        public paginationviewmodel pagination {get; set;}
        public AddModifierViewModel AddModifier { get; set; }
        public ModifierGroupViewModel AddModifierGroup { get; set; }

}

        

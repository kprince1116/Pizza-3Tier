using DAL.Models;

namespace Pizzashop.DAL.ViewModels;

public class MenuItemsviewmodel
{
    public int Itemid { get; set; }

    public int? Categoryid { get; set; }

    public string Itemname { get; set; } = null!;

    public string? Itemtype { get; set; }

    public int? Quantity { get; set; }

    public decimal? Rate { get; set; }

    public bool IsAvailable { get; set; }

    public string? Image { get; set; }
    public string? Description { get; set; }

    public decimal? TaxPercentage { get; set; }

    public bool? IsFavourite { get; set; }

    public string? ShortCode { get; set; }

    public bool IsDefaultTax { get; set; }

    public virtual MenuCategory? Category { get; set; }

    public virtual Unit? Unit { get; set; }

}

// all about Modifier Group & item --------------------------------------
// public class ModifierGroupModel
// {
//     public int Modifiergroupid { get; set; }

//     public string Modifiergroupname { get; set; }

//     public string Description { get; set; }
// }

// public class ModifiersViewMenuModel
// {
//     public int Modgroupid { get; set; }

//     public int Pagenumber {get; set;}

//     public int Pagesize {get; set;}

//     public string Searchkey {get; set;}

//     public int Count{get; set;}

//     public List<SingleModifier> modifiersList {get; set;}
// }

// public class SingleModifier
// {
//     public int ModifierId {get; set;}

//     public string Modifiername {get; set;}

//     public string Unit {get; set;}

//     public short Rate { get; set; }

//     public int Quantity { get; set; }

//     public string ImageUrl {get; set;}
// }

// public class AddEditDeleteModGroup
// {
//     public string Modgroupid {get; set;}

//     public string Modgroupname { get; set; }

//     public string Description { get; set; }

//     public List<string> Modifierslist {get; set;}

//     public List<int> Modifiersidlist {get; set;}

//     public int CreatedBy {get; set;}

//     // public int ModifiedBy {get; set;}

//     public DateTime ModifiedDate{get; set;}
// }

// public class AddEditDeleteModifiers
// {
//     public int ModifierId {get; set;}

//     public string Modifiername {get; set;}

//     public string Unit {get; set;}

//     public short Rate { get; set; }

//     public int Quantity { get; set; }

//     public string Description {get; set;}

//     public List<string> ModifiersGroupList{get; set;}

//     public List<int> ModifiersGroupListIds{get; set;}


//     public int ModifiedBy{get; set;}
// }


// public class ModGroupDetails
// {
//     public int Id{get; set;}
//     public string GroupName{get; set;}
//     public int Min{get; set;}
//     public int Max{get; set;}
//     public List<itemForList> Items{get; set;}
// }

// public class itemForList
// {
//     public string ItemName{get; set;}
//     public short Rate { get; set; }
// }
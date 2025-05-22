namespace Pizzashop.DAL.ViewModels;

public class KotOrderCardDTOViewModel
{
    public int orderid { get; set; }
    public int orderno { get; set; }
    public string orderstatus { get; set; }
    public int itemid { get; set; }
    public int orderitemid {get; set;}
    public int categoryid {get; set;}
    public string itemname { get; set; }
    public bool isselected { get; set; } = false;
    public int totalquantity { get; set; }
    public int readyquantity { get; set; }
    public int pendingquantity { get; set; }
    public int modifierid { get; set; }
    public string modifiername { get; set; }
}

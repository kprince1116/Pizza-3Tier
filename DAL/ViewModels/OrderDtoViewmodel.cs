namespace Pizzashop.DAL.ViewModels;

public class OrderDtoViewmodel
{
    public int OrderId { get; set; }
    public int OrderNo { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderStatus { get; set; }
    public string OrderInstruction { get; set; }
    public string TableNo { get; set; }
    public string SectionName { get; set; }
    public int OrderItemId { get; set; }
    public string ItemName { get; set; }
    public int Quantity { get; set; }
    public int PendingQuantity { get; set; }
    public int ReadyQuantity { get; set; }
    public string ItemInstruction { get; set; }
    public int CategoryId { get; set; }
    public int oid {get; set;}
    public string ModifierName { get; set; }
}
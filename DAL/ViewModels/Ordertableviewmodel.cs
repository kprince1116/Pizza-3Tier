namespace Pizzashop.DAL.ViewModels;

public class Ordertableviewmodel
{
    public int Orderid { get; set; }

    public int OrderNo { get; set; }

    public DateTime? Orderdate { get; set; }

    public int? CustomerId { get; set; }

    public string CustomerName {get; set;}

    public int? StatusId {get; set;}

    public string Status {get; set;}

    public int? PaymentMode { get; set; }

    public string Payment {get; set;}

    public short? Rating { get; set; }

    public decimal? TotalAmount { get; set; }
   
}

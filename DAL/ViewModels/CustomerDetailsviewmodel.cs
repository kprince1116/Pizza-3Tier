namespace Pizzashop.DAL.ViewModels;

public class CustomerDetailsviewmodel
{
     public int Customerid { get; set; }

    public string Customername { get; set; } 

    public string? Customeremail { get; set; }

    public string? Phonenumber { get; set; }

    public DateTime? CutomerDate { get; set; }

    public int? TotalOrders { get; set; }

     
}

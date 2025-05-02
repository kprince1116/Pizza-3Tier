using DAL.Models;

namespace Pizzashop.DAL.ViewModels;

public class CustomerHistoryviewmodel
{
    public string CustomerName {get; set;}
    public decimal avgBill {get; set;}
    public string Phonenumber {get;set;}
    public DateTime comingsince  {get;set;}
    public decimal maxOrder  {get;set;}
    public int totalvisits {get; set;}
    public List<Orderdata> OrderList = new List<Orderdata> ();
}

public class Orderdata
{
    public DateTime OrderDate {get; set;}
    public string OrderType {get; set;}
    public string paymentmode {get; set;}
    public int totalitems {get;set;}
    public decimal totalamount {get;set;}
}
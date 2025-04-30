namespace Pizzashop.DAL.ViewModels;

public class dashboardviewmodel
{
    public decimal TotalSales {get; set;}
    public int TotalOrders {get; set;}
    public decimal AvgOrderValue {get;set;}
    public decimal AvgWaitingTime {get;set;}
    public int WaitingListCount {get; set;}
    public int NewCustomerCount {get; set;}
    public List<ItemDashboardviewmodel> topSellingItems {get; set;}
    public List<ItemDashboardviewmodel> leastSellingItems {get; set;}
    public List<Revenueviewmodel> RevenueList {get; set;}
    public List<CustomerDashboardviewmodel> CustomerList {get; set;}
}

public class ItemDashboardviewmodel
{
     public int ItemId  {get; set;}
     public string ItemName {get; set;}
     public int OrderCount {get; set;}
     public string ItemImage {get; set;}
}

public class Revenueviewmodel
{
    public DateTime RevenueDate {get; set;}
    public decimal TotalRevenue {get; set;}
}

public class CustomerDashboardviewmodel
{
    public DateTime CustomerDate {get; set;}
    public decimal TotalCustomer {get; set;}
}
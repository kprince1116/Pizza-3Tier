using BAL.Models.Interfaces;
using DAL.Interfaces;
using Microsoft.Identity.Client;
using Pizzashop.DAL.ViewModels;

namespace BAL.Services;

public class Dashboard : IDashboard
{
    private readonly IDashboardRepository _dashboardRepository;

    public Dashboard(IDashboardRepository dashboardRepository)
    {
         _dashboardRepository =  dashboardRepository;
    }

    public async Task<dashboardviewmodel> GetDashboardData(string time ,  string fromdate , string todate)
    {
        
        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.MaxValue;

        if(!string.IsNullOrEmpty(time))
        {
              switch(time.ToLower())
             {
                case "today":
                startDate = DateTime.Today;
                endDate = DateTime.Today.AddDays(1).AddTicks(-1);
                break;
    
                case "this_month":
                startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                endDate = startDate.AddMonths(1).AddTicks(-1);
                break;
    
                case "last_7_days":
                startDate = DateTime.Today.AddDays(-7);
                endDate = DateTime.Today.AddDays(1).AddTicks(-1);
                break;
    
                case "last_30_days":
                startDate = DateTime.Today.AddDays(-30);
                endDate = DateTime.Today.AddDays(1).AddTicks(-1); 
                break;
             }  
        } 

        if(!string.IsNullOrEmpty(fromdate)  && !string.IsNullOrEmpty(todate))
        {
            startDate = DateTime.Parse(fromdate);
            endDate = DateTime.Parse(todate); 
        }

        
        dashboardviewmodel obj = new();
        obj.TotalSales = await _dashboardRepository.GetTotalSales(startDate, endDate);
        obj.TotalOrders = await _dashboardRepository.GetTotalOrders(startDate, endDate);
        obj.AvgOrderValue = await _dashboardRepository.GetAvgOrderValue(startDate, endDate);
        obj.AvgWaitingTime = await _dashboardRepository.GetAvgWaitingTime(startDate, endDate);
        obj.WaitingListCount = await _dashboardRepository.GetWaitingListCount(startDate, endDate);
        obj.NewCustomerCount = await _dashboardRepository.GetNewCustomerCount(startDate, endDate);
        obj.topSellingItems = await _dashboardRepository.GetTopSellingItems(startDate,endDate);
        obj.leastSellingItems = await _dashboardRepository.GetLeastSellingItems(startDate,endDate);
        obj.RevenueList = await _dashboardRepository.GetRevenueList(startDate,endDate, time );
        obj.CustomerList = await _dashboardRepository.GetCustomerList(startDate,endDate);

        return obj;
    }
}

using Pizzashop.DAL.ViewModels;

namespace BAL.Models.Interfaces;

public interface IDashboard
{
    public Task<dashboardviewmodel> GetDashboardData(string time, string startdate , string enddate);
}

using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Pizzashop.DAL.ViewModels;

namespace DAL.Repository;

public class DashboardRepository : IDashboardRepository
{
    private readonly PizzaShopContext _db;

    public DashboardRepository(PizzaShopContext db)
    {
        _db = db;
    }
    public async Task<int> GetTotalOrders(DateTime startDate, DateTime endDate)
    {
        try
        {
            var orders = _db.Orders.Where(u => u.Status == 2 && u.Isdelete == false && u.Orderdate.HasValue && u.Orderdate.Value >= startDate && u.Orderdate.Value <= endDate);

            int totalorder = await orders.CountAsync();

            return totalorder;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return 0;
        }

    }

    public async Task<decimal> GetTotalSales(DateTime startDate, DateTime endDate)
    {
        try
        {
            var totalsales = _db.Paymentmodes.Where(p => p.PaymentStatus == "paid" && p.PaidOn.HasValue && p.PaidOn.Value >= startDate && p.PaidOn.Value <= endDate);

            decimal totalamount = 0;

            totalamount = (decimal)await totalsales.SumAsync(u => u.Totalamount);
            return totalamount;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return 0;
        }
    }


    public async Task<decimal> GetAvgOrderValue(DateTime startDate, DateTime endDate)
    {
        try
        {
            var totalsales = _db.Paymentmodes.Where(p => p.PaymentStatus == "paid" && p.PaidOn.HasValue && p.PaidOn.Value >= startDate && p.PaidOn.Value <= endDate);


            decimal avgamount = 0;

            avgamount = Math.Round((decimal)await totalsales.AverageAsync(u => u.Totalamount), 2);

            return avgamount;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return 0;
        }

    }

    public async Task<decimal> GetAvgWaitingTime(DateTime startDate, DateTime endDate)
    {
        try
        {
            var waiting = _db.WaitingTokens.Where(u => u.IsAssigned == true && u.CreatedDate.HasValue && u.CreatedDate.Value >= startDate && u.CreatedDate.Value <= endDate);

            var waitinglist = await waiting.Select(u => (u.AssignedTime.Value - u.CreatedDate.Value).TotalMinutes).ToListAsync();

            decimal avgwaitingtime = 0;

            avgwaitingtime = Math.Round((decimal)waitinglist.Average(), 2);

            return avgwaitingtime;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return 0;
        }

    }

    public async Task<int> GetNewCustomerCount(DateTime startDate, DateTime endDate)
    {
        try
        {
            var Customers = _db.Customers.Where(u => u.Isdelete == false && u.CreatedDate.HasValue && u.CreatedDate.Value >= startDate && u.CreatedDate.Value <= endDate);


            int customercount = await Customers.CountAsync();

            return customercount;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return 0;
        }

    }


    public async Task<int> GetWaitingListCount(DateTime startDate, DateTime endDate)
    {
        try
        {
            var waitingcount = _db.WaitingTokens.Where(u => u.IsDeleted == false && u.CreatedDate.HasValue && u.CreatedDate.Value >= startDate && u.CreatedDate.Value <= endDate);

            int totalawaiting = await waitingcount.CountAsync();

            return totalawaiting;
        }
        catch (Exception e)
        {
            return 0;
        }
    }

    public async Task<List<OrderItem>> GetItemList(DateTime startDate, DateTime endDate)
    {
        return null;
    }


    public async Task<List<ItemDashboardviewmodel>> GetTopSellingItems(DateTime startDate, DateTime endDate)
    {
        var orderItem = _db.OrderItems.Include(u => u.Item).Include(u => u.Order).Where(u => u.CreatedDate.HasValue && u.CreatedDate.Value >= startDate && u.CreatedDate.Value <= endDate);

        var itemList = orderItem.GroupBy(u => u.ItemId).Select
        (u => new ItemDashboardviewmodel
        {
            ItemId = (int)u.First().ItemId,
            ItemName = u.First().Item.Itemname,
            OrderCount = u.Count(),
            ItemImage = u.First().Item.Image
        }).OrderByDescending(u => u.OrderCount).Take(2).ToList();

        return itemList;
    }
    public async Task<List<ItemDashboardviewmodel>> GetLeastSellingItems(DateTime startDate, DateTime endDate)
    {
        var orderItem = _db.OrderItems.Include(u => u.Item).Include(u => u.Order).Where(u => u.CreatedDate.HasValue && u.CreatedDate.Value >= startDate && u.CreatedDate.Value <= endDate);

        var itemList = orderItem.GroupBy(u => u.ItemId).Select
       (u => new ItemDashboardviewmodel
       {
           ItemId = (int)u.First().ItemId,
           ItemName = u.First().Item.Itemname,
           OrderCount = u.Count(),
           ItemImage = u.First().Item.Image
       }).OrderBy(u => u.OrderCount).Take(2).ToList();

        return itemList;
    }

    public async Task<List<Revenueviewmodel>> GetRevenueList(DateTime startDate, DateTime endDate, string time)
    {
        try
        {
            var payments = _db.Paymentmodes.Where(u => u.PaymentStatus == "paid" && u.PaidOn.HasValue && u.PaidOn.Value >= startDate && u.PaidOn.Value <= endDate).OrderBy(u => u.PaidOn);

            var revenuelist = new List<Revenueviewmodel>();
            if(startDate.Date == endDate.Date)
            {
             revenuelist = payments.GroupBy(u => u.PaidOn.Value.Hour).Select(u => new Revenueviewmodel
            {
                RevenueDate = (DateTime)u.First().PaidOn,
                TotalRevenue = (decimal)u.Sum(u => u.Totalamount)
            }).ToList();
            }
            else{
             revenuelist = payments.GroupBy(u => u.PaidOn.Value.Date).Select(u => new Revenueviewmodel
            {
                RevenueDate = (DateTime)u.First().PaidOn,
                TotalRevenue = (decimal)u.Sum(u => u.Totalamount)
            }).ToList();
            }

            Dictionary<DateTime, decimal> revenueDist;
            List<Revenueviewmodel> newRevenue = new();

            switch (time)
            {
                case "today":
                    newRevenue = Enumerable.Range(0, 24)
                    .Select(hour => new Revenueviewmodel
                    {
                        RevenueDate = startDate.Date.AddHours(hour),
                        TotalRevenue = revenuelist.Where(u => u.RevenueDate.Hour == hour && u.RevenueDate.Date == startDate).Sum(u => u.TotalRevenue)
                    }).ToList();
                    break;
                case "Last_7_days":
                case "Last_30_days":
                    revenueDist = revenuelist.GroupBy(u => u.RevenueDate.Date).ToDictionary(u => u.Key, u => u.Sum(x => x.TotalRevenue));
                    int totalDays = (endDate - startDate).Days;
                    newRevenue = Enumerable.Range(0, totalDays + 1)
                    .Select(day => new Revenueviewmodel
                    {
                        RevenueDate = startDate.Date.AddDays(day),
                        TotalRevenue = revenueDist.ContainsKey(startDate.Date.AddDays(day)) ? revenueDist[startDate.Date.AddDays(day)] : 0
                    }).ToList();
                    break;
                case "this_month":
                    revenueDist = revenuelist.GroupBy(u => new { u.RevenueDate.Year, u.RevenueDate.Month , u.RevenueDate.Day }).ToDictionary(
                        g => new DateTime(g.Key.Year, g.Key.Month, g.Key.Day),
                        g => g.Sum(x => x.TotalRevenue)
                    );
                    DateTime itr = new(startDate.Year, startDate.Month, 1);
                    DateTime lastMonth = new(endDate.Year, endDate.Month, endDate.Day);

                    while (itr <= lastMonth)
                    {
                        newRevenue.Add(new Revenueviewmodel
                        {
                            RevenueDate = itr,
                            TotalRevenue = revenueDist.ContainsKey(itr) ? revenueDist[itr] : 0
                        });
                        itr = itr.AddDays(1);
                    }
                    break;
                default:
                    startDate = revenuelist.Min(u => u.RevenueDate);
                    endDate = revenuelist.Max(u => u.RevenueDate);
                    totalDays = (endDate - startDate).Days;
                    bool isSameYear = startDate.Year == endDate.Year;
                    bool isSameMonth = startDate.Month == endDate.Month;

                    if (!isSameYear)
                    {
                        revenueDist = revenuelist.GroupBy(u => u.RevenueDate.Year).ToDictionary(
                            u => new DateTime(u.Key, 1, 1),
                            u => u.Sum(x => x.TotalRevenue)
                        );
                        for (int year = startDate.Year; year <= endDate.Year; year++)
                        {
                            DateTime yearstart = new DateTime(year, 1, 1);
                            newRevenue.Add(new Revenueviewmodel
                            {
                                RevenueDate = yearstart,
                                TotalRevenue = revenueDist.ContainsKey(yearstart) ? revenueDist[yearstart] : 0
                            });
                        }
                    }
                    else if (!isSameMonth)
                    {
                        revenueDist = revenuelist.GroupBy(u => new { u.RevenueDate.Year, u.RevenueDate.Month }).ToDictionary(
                           g => new DateTime(g.Key.Year, g.Key.Month, 1),
                           g => g.Sum(x => x.TotalRevenue)
                       );
                        itr = new(startDate.Year, startDate.Month, 1);
                        lastMonth = new(endDate.Year, endDate.Month, 1);
                        while (itr <= lastMonth)
                        {
                            newRevenue.Add(new Revenueviewmodel
                            {
                                RevenueDate = itr,
                                TotalRevenue = revenueDist.ContainsKey(itr) ? revenueDist[itr] : 0
                            });
                            itr = itr.AddMonths(1);
                        }
                    }
                    else
                    {
                        revenueDist = revenuelist
                                        .GroupBy(u => u.RevenueDate.Date)
                                        .ToDictionary(u => u.Key, u => u.Sum(x => x.TotalRevenue));
                        totalDays = (endDate - startDate).Days;
                        newRevenue = Enumerable.Range(0, totalDays + 1).Select(i => new Revenueviewmodel
                        {
                            RevenueDate = startDate.Date.AddDays(i),
                            TotalRevenue = revenueDist.ContainsKey(startDate.Date.AddDays(i)) ? revenueDist[startDate.Date.AddDays(i)] : 0
                        }).ToList();

                    }

                    break;
            }

            return newRevenue.OrderBy(u => u.RevenueDate).ToList();
        }
        catch (Exception e)
        {
            return null;
        }


    }

    public async Task<List<CustomerDashboardviewmodel>> GetCustomerList(DateTime startDate, DateTime endDate, string time)
    {
        try
        {
            var customers = _db.Customers
        .Where(u => u.CreatedDate.HasValue && u.CreatedDate.Value >= startDate && u.CreatedDate.Value <= endDate)
        .OrderBy(u => u.CreatedDate);

        var customerlist = new List<CustomerDashboardviewmodel>();

        if(startDate.Date == endDate.Date)
        {
             customerlist = customers
            .GroupBy(u => u.CreatedDate.Value.Hour)
            .Select(u => new CustomerDashboardviewmodel
            {
                CustomerDate = (DateTime)u.First().CreatedDate,
                TotalCustomer = u.Count()
            })
            .ToList();
        }
        else{
             customerlist = customers
            .GroupBy(u => u.CreatedDate.Value.Date)
            .Select(u => new CustomerDashboardviewmodel
            {
                CustomerDate = (DateTime)u.First().CreatedDate,
                TotalCustomer = u.Count()
            })
            .ToList();

        }

            
            Dictionary<DateTime, decimal> customerCount;
            List<CustomerDashboardviewmodel> newCustomer = new();

            switch (time)
            {
                case "today":
                    newCustomer = Enumerable.Range(0, 24)
                    .Select(hour => new CustomerDashboardviewmodel
                    {
                        CustomerDate = startDate.Date.AddHours(hour),
                        TotalCustomer = customerlist.Where(u => u.CustomerDate.Hour == hour && u.CustomerDate.Date == startDate).Sum(u => u.TotalCustomer)
                    }).ToList();
                    break;

                case "Last_7_days":
                case "Last_30_days":
                    customerCount = customerlist.GroupBy(u => u.CustomerDate.Date).ToDictionary(u => u.Key, u => u.Sum(x => x.TotalCustomer));
                    int totalDays = (endDate - startDate).Days;
                    newCustomer = Enumerable.Range(0, totalDays + 1)
                    .Select(day => new CustomerDashboardviewmodel
                    {
                        CustomerDate = startDate.Date.AddDays(day),
                        TotalCustomer = customerCount.ContainsKey(startDate.Date.AddDays(day)) ? customerCount[startDate.Date.AddDays(day)] : 0
                    }).ToList();
                    break;
                case "this_month":
                    customerCount = customerlist.GroupBy(u => new { u.CustomerDate.Year, u.CustomerDate.Month , u.CustomerDate.Day}).ToDictionary(
                        g => new DateTime(g.Key.Year, g.Key.Month, g.Key.Day),
                        g => g.Sum(x => x.TotalCustomer)
                    );
                    DateTime itr = new(startDate.Year, startDate.Month, 1);
                    DateTime lastmonth = new(endDate.Year, endDate.Month, endDate.Day);

                    while (itr <= lastmonth)
                    {
                        newCustomer.Add(new CustomerDashboardviewmodel
                        {
                            CustomerDate = itr,
                            TotalCustomer = customerCount.ContainsKey(itr) ? customerCount[itr] : 0

                        });
                        itr = itr.AddDays(1);
                    }
                    break;
                default:

                    startDate = customerlist.Min(u => u.CustomerDate);
                    endDate = customerlist.Max(u => u.CustomerDate);
                    totalDays = (endDate - startDate).Days;
                    bool isSameYear = startDate.Year == endDate.Year;
                    bool isSameMonth = startDate.Month == endDate.Month;

                    if (!isSameYear)
                    {
                        customerCount = customerlist.GroupBy(u => u.CustomerDate.Year).ToDictionary(
                            u => new DateTime(u.Key, 1, 1),
                            u => u.Sum(x => x.TotalCustomer)
                        );
                        for (int year = startDate.Year; year <= endDate.Year; year++)
                        {
                            DateTime yearstart = new DateTime(year, 1, 1);
                            newCustomer.Add(new CustomerDashboardviewmodel
                            {
                                CustomerDate = yearstart,
                                TotalCustomer = customerCount.ContainsKey(yearstart) ? customerCount[yearstart] : 0
                            });
                        }
                    }
                    else if (!isSameMonth && totalDays >= 30)
                    {
                        customerCount = customerlist.GroupBy(u => new { u.CustomerDate.Year, u.CustomerDate.Month }).ToDictionary(
                          g => new DateTime(g.Key.Year, g.Key.Month, 1),
                          g => g.Sum(x => x.TotalCustomer)
                      );
                        itr = new(startDate.Year, startDate.Month, 1);
                        lastmonth = new(endDate.Year, endDate.Month, 1);
                        while (itr <= lastmonth)
                        {
                            newCustomer.Add(new CustomerDashboardviewmodel
                            {
                                CustomerDate = itr,
                                TotalCustomer = customerCount.ContainsKey(itr) ? customerCount[itr] : 0
                            });
                            itr = itr.AddMonths(1);
                        }
                    }
                    else
                    {
                        customerCount = customerlist
                                        .GroupBy(u => u.CustomerDate.Date)
                                        .ToDictionary(u => u.Key, u => u.Sum(x => x.TotalCustomer));
                        totalDays = (endDate - startDate).Days;
                        newCustomer = Enumerable.Range(0, totalDays + 1).Select(i => new CustomerDashboardviewmodel
                        {
                            CustomerDate = startDate.Date.AddDays(i),
                            TotalCustomer = customerCount.ContainsKey(startDate.Date.AddDays(i)) ? customerCount[startDate.Date.AddDays(i)] : 0
                        }).ToList();

                    }

                    break;

            }
            return newCustomer.ToList();
        }
        catch (Exception e)
        {
            return null;
        }

    }


}
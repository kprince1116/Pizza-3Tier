using BAL.Models.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pizzashop.DAL.ViewModels;

namespace BAL.Services;

public class Orderservice : IOrderservice
{
    private readonly IOrderRepository _orderRepository;

    public Orderservice(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Orderviewmodel> GetOrderDetails(int pageNo, int pageSize, string searchKey, string sortBy, string sortDirection, string statusFilter, string timeFilter, string fromDate = "", string toDate = "")
    {
        try
        {
            var orders = await _orderRepository.GetOrderDetails(pageNo, pageSize, searchKey, sortBy, sortDirection, statusFilter, timeFilter, fromDate, toDate);

            var mappedOrders = orders.Select(u => new Ordertableviewmodel
            {
                Orderid = u.Orderid,
                OrderNo = u.OrderNo,
                Orderdate = u.CreatedDate,
                CustomerName = u.Customer?.Customername ?? string.Empty,
                Status = u.StatusNavigation?.Status ?? string.Empty,
                Payment = u.PaymentModeNavigation != null ? u.PaymentModeNavigation.Status : "Cancelled",
                Rating = u.RatingNavigation?.Avgrating ?? 0,
                TotalAmount = u.TotalAmount ?? 0
            }).ToList();

            if (!string.IsNullOrEmpty(searchKey))
            {
                var lowerSearchQuery = searchKey.ToLower();
                mappedOrders = mappedOrders.Where(o =>
                    o.CustomerName.ToLower().Contains(lowerSearchQuery) ||
                    o.OrderNo.ToString().ToLower().Contains(lowerSearchQuery) ||
                    o.Status.ToLower().Contains(lowerSearchQuery)).ToList();
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                mappedOrders = mappedOrders.Where(u => u.Status.ToLower() == statusFilter.ToLower()).ToList();
            }

            if (!string.IsNullOrEmpty(timeFilter))
            {
                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MaxValue;

                switch (timeFilter.ToLower())
                {
                    case "today":
                        startDate = DateTime.Today;
                        endDate = DateTime.Today.AddDays(1).AddTicks(-1);
                        break;

                    case "this_week":
                        startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                        endDate = startDate.AddDays(7).AddTicks(-1);
                        break;

                    case "this_month":
                        startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                        endDate = startDate.AddMonths(1).AddTicks(-1);
                        break;
                }

                if (startDate != DateTime.MinValue && endDate != DateTime.MaxValue)
                {
                    mappedOrders = mappedOrders.Where(o => o.Orderdate.HasValue && o.Orderdate.Value >= startDate && o.Orderdate.Value <= endDate).ToList();
                }
            }

            if (!string.IsNullOrEmpty(fromDate) && DateTime.TryParse(fromDate, out var fromDateTime))
            {
                mappedOrders = mappedOrders.Where(o => o.Orderdate.HasValue && o.Orderdate.Value.Date >= fromDateTime.Date).ToList();
            }

            if (!string.IsNullOrEmpty(toDate) && DateTime.TryParse(toDate, out var toDateTime))
            {
                mappedOrders = mappedOrders.Where(o => o.Orderdate.HasValue && o.Orderdate.Value.Date <= toDateTime.Date).ToList();
            }

            switch (sortBy)
            {
                case "OrderNo":
                    mappedOrders = (sortDirection == "asc") ? mappedOrders.OrderBy(u => u.OrderNo).ToList() : mappedOrders.OrderByDescending(u => u.OrderNo).ToList();
                    break;
                case "Orderdate":
                    mappedOrders = (sortDirection == "asc") ? mappedOrders.OrderBy(u => u.Orderdate).ToList() : mappedOrders.OrderByDescending(u => u.Orderdate).ToList();
                    break;
                case "CustomerName":
                    mappedOrders = (sortDirection == "asc") ? mappedOrders.OrderBy(u => u.CustomerName).ToList() : mappedOrders.OrderByDescending(u => u.CustomerName).ToList();
                    break;
                case "TotalAmount":
                    mappedOrders = (sortDirection == "asc") ? mappedOrders.OrderBy(u => u.TotalAmount).ToList() : mappedOrders.OrderByDescending(u => u.TotalAmount).ToList();
                    break;
                default:
                    mappedOrders = mappedOrders.OrderByDescending(u => u.Orderid).ToList();
                    break;
            }

            var totalRecords = mappedOrders.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            var paginatedOrders = mappedOrders.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

            return new Orderviewmodel
            {
                Orders = paginatedOrders,
                CurrentPage = pageNo,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<List<Ordertableviewmodel>> GetOrder(string searchKey, string statusFilter, string timeFilter)
    {
        try
        {
            var orders = await _orderRepository.GetOrder(searchKey, statusFilter, timeFilter);

            var mappedOrders = orders.Select(u => new Ordertableviewmodel
            {
                Orderid = u.Orderid,
                OrderNo = u.OrderNo,
                Orderdate = u.Orderdate,
                CustomerName = u.Customer?.Customername ?? string.Empty,
                Status = u.StatusNavigation?.Status ?? string.Empty,
                Payment = u.PaymentModeNavigation?.Status ?? "Cancelled",
                Rating = u.Rating ?? 0,
                TotalAmount = u.TotalAmount ?? 0
            }).AsQueryable();

            if (!string.IsNullOrEmpty(searchKey))
            {
                var lowerSearchQuery = searchKey.ToLower();
                mappedOrders = mappedOrders.Where(o =>
                    o.CustomerName.ToLower().Contains(lowerSearchQuery) ||
                    o.OrderNo.ToString().ToLower().Contains(lowerSearchQuery) ||
                    o.Status.ToLower().Contains(lowerSearchQuery));
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                mappedOrders = mappedOrders.Where(u => u.Status.ToLower() == statusFilter.ToLower());
            }

            if (!string.IsNullOrEmpty(timeFilter))
            {
                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MaxValue;

                switch (timeFilter.ToLower())
                {
                    case "today":
                        startDate = DateTime.Today;
                        endDate = DateTime.Today.AddDays(1).AddTicks(-1);
                        break;

                    case "this_week":
                        startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                        endDate = startDate.AddDays(7).AddTicks(-1);
                        break;

                    case "this_month":
                        startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                        endDate = startDate.AddMonths(1).AddTicks(-1);
                        break;
                }

                if (startDate != DateTime.MinValue && endDate != DateTime.MaxValue)
                {
                    mappedOrders = mappedOrders.Where(o =>
                        o.Orderdate.HasValue && o.Orderdate.Value >= startDate && o.Orderdate.Value <= endDate);
                }
            }

            return mappedOrders.OrderBy(u => u.Orderid).ToList();
        }
        catch (Exception ex)
        {
            return new List<Ordertableviewmodel>();
        }
    }

    public async Task<Order> GetDetails(int orderId)
    {
        return await _orderRepository.GetDetails(orderId);
    }


}

using System.ComponentModel;
using BAL.Models.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace BAL.Services;

public class CustomerService : ICustomerService
{

    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customerviewmodel> GetCustomerDetails(int pageNo, int pageSize, string searchKey, string sortby, string sortdirection, string timefilter, string fromdate, string todate)
    {
        try
        {
            var customers = await _customerRepository.GetCustomerDetails(pageNo, pageSize, searchKey, sortby, sortdirection, timefilter, fromdate, todate);

            var mappedCustomers = customers.Select(u => new CustomerDetailsviewmodel
            {
                Customerid = u.Customerid,
                Customername = u.Customername ?? string.Empty,
                Customeremail = u.Customeremail ?? string.Empty,
                Phonenumber = u.Phonenumber ?? string.Empty,
                CutomerDate = u.CreatedDate,
                TotalOrders = u.Orders?.Count() ?? 0
            }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                var lowerSearchQuery = searchKey.Trim().ToLower();
                mappedCustomers = mappedCustomers.Where(o =>
                    (!string.IsNullOrEmpty(o.Customeremail) && o.Customeremail.ToLower().Contains(lowerSearchQuery)) ||
                    (!string.IsNullOrEmpty(o.Customername) && o.Customername.ToLower().Contains(lowerSearchQuery)) ||
                    (!string.IsNullOrEmpty(o.Phonenumber) && o.Phonenumber.ToLower().Contains(lowerSearchQuery)));
            }

            switch (sortby?.ToLower())
            {
                case "customername":
                    mappedCustomers = (sortdirection == "asc") ? mappedCustomers.OrderBy(c => c.Customername) : mappedCustomers.OrderByDescending(c => c.Customername);
                    break;

                case "cutomerdate":
                    mappedCustomers = (sortdirection == "asc") ? mappedCustomers.OrderBy(c => c.CutomerDate) : mappedCustomers.OrderByDescending(c => c.CutomerDate);
                    break;

                case "totalorders":
                    mappedCustomers = (sortdirection == "asc") ? mappedCustomers.OrderBy(c => c.TotalOrders) : mappedCustomers.OrderByDescending(c => c.TotalOrders);
                    break;

                default:
                    mappedCustomers = mappedCustomers.OrderBy(c => c.Customerid);
                    break;
            }

            if (!string.IsNullOrEmpty(timefilter))
            {
                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MaxValue;

                switch (timefilter.ToLower())
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
                    mappedCustomers = mappedCustomers.Where(c => c.CutomerDate.HasValue && c.CutomerDate.Value >= startDate && c.CutomerDate.Value <= endDate);
                }
            }

            if (!string.IsNullOrEmpty(fromdate) && DateTime.TryParse(fromdate, out var fromDateTime))
            {
                mappedCustomers = mappedCustomers.Where(c => c.CutomerDate.HasValue && c.CutomerDate.Value.Date >= fromDateTime.Date);
            }

            if (!string.IsNullOrEmpty(todate) && DateTime.TryParse(todate, out var toDateTime))
            {
                mappedCustomers = mappedCustomers.Where(c => c.CutomerDate.HasValue && c.CutomerDate.Value.Date <= toDateTime.Date);
            }

            var totalRecords = mappedCustomers.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            var customerList = mappedCustomers.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

            return new Customerviewmodel
            {
                Customers = customerList,
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

    public async Task<List<CustomerDetailsviewmodel>> GetCustomer(string searchKey, string timefilter, string fromdate, string todate)
    {
        try
        {
            var customers = await _customerRepository.GetCustomer(searchKey, timefilter, fromdate, todate);

            var mappedCustomers = customers.Select(u => new CustomerDetailsviewmodel
            {
                Customerid = u.Customerid,
                Customername = u.Customername, 
                Customeremail = u.Customeremail , 
                Phonenumber = u.Phonenumber, 
                CutomerDate = u.CreatedDate, 
                TotalOrders = u.Orders?.Count() 
            }).AsQueryable(); 

            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                var lowerSearchQuery = searchKey.Trim().ToLower();
                mappedCustomers = mappedCustomers.Where(o =>
                    (!string.IsNullOrEmpty(o.Customeremail) && o.Customeremail.ToLower().Contains(lowerSearchQuery)) ||
                    (!string.IsNullOrEmpty(o.Customername) && o.Customername.ToLower().Contains(lowerSearchQuery)) ||
                    (!string.IsNullOrEmpty(o.Phonenumber) && o.Phonenumber.ToLower().Contains(lowerSearchQuery)));
            }

            if (!string.IsNullOrEmpty(timefilter))
            {
                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MaxValue;

                switch (timefilter.ToLower())
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
                    mappedCustomers = mappedCustomers.Where(c => c.CutomerDate.HasValue && c.CutomerDate.Value >= startDate && c.CutomerDate.Value <= endDate);
                }
            }

            if (!string.IsNullOrEmpty(fromdate) && DateTime.TryParse(fromdate, out var fromDateTime))
            {
                mappedCustomers = mappedCustomers.Where(c => c.CutomerDate.HasValue && c.CutomerDate.Value.Date >= fromDateTime.Date);
            }

            if (!string.IsNullOrEmpty(todate) && DateTime.TryParse(todate, out var toDateTime))
            {
                mappedCustomers = mappedCustomers.Where(c => c.CutomerDate.HasValue && c.CutomerDate.Value.Date <= toDateTime.Date);
            }

            return mappedCustomers.OrderBy(u=>u.Customerid).ToList();
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public async Task<CustomerHistoryviewmodel> GetCustomerHistory(int id)
    {
        try
        {
            var customer = await _customerRepository.GetCustomerHistory(id);

            var viewmodel = new CustomerHistoryviewmodel
            {
                comingsince = (DateTime)customer.CreatedDate,
                CustomerName = customer.Customername,
                Phonenumber = customer.Phonenumber,
                totalvisits = customer.Orders.Count,
                avgBill = customer.Orders.Any()
                ? (decimal)customer.Orders.Where(u => u.PaymentMode != null).Sum(u => u.TotalAmount) / customer.Orders.Count
                : 0,
                maxOrder = customer.Orders.Any(u => u.PaymentMode != null)
                ? (decimal)customer.Orders.Where(u => u.PaymentMode != null).Select(u => u.TotalAmount).Max()
                : 0,

                OrderList = customer.Orders.Select(u => new Orderdata
                {
                    OrderDate = (DateTime)u.Orderdate,
                    OrderType = "Dineln",
                    paymentmode = u.PaymentModeNavigation?.Status ?? "Cancelled",
                    totalamount = u.TotalAmount != null ? (decimal)u.TotalAmount : 0,
                    totalitems = u.OrderItems.Count()
                }).ToList()

            };
            return viewmodel;
        }
        catch (Exception e)
        {
            return null;
        }

    }

}

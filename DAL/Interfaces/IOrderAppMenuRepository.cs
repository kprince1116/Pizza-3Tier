using DAL.Models;
using DAL.Repository;
using Pizzashop.DAL.ViewModels;

namespace DAL.Interfaces;

public interface IOrderAppMenuRepository 
{
    Task<List<MenuCategory>> GetCategories();

    Task<List<MenuItem>> GetItems(int CategoryId,string SearchKey);

    Task<List<MenuItem>> GetFavouriteItems(string SearchKey);

    Task<MenuItem> GetItemById(int ItemId);

    Task UpdateItem(MenuItem item);

    Task<MenuItem> GetItemForModalById(int ItemId);

    Task<Order> GetTableData( int OrderId);

    Task<List<OrderItem>> GetOrderItems(int OrderId);

    Task<Order> GetCustomerDetails(int OrderId);

    Task<Customer> GetCustomerById(int CustomerId);

    Task<Order> GetOrderDetails(int OrderId);

    Task UpdateCustomer(Customer customer);

    Task UpdateOrder(Order order);

    Task<Order> GetOrderComments(int OrderId);
    Task<OrderItem> GetItemComments(int OrderId);

    Task<List<Taxesandfess>> GetTaxList();

    Task<List<OrderTax>> GetTaxLists(int OrderId);

    Task<Table> GetTable(int TableId);

    Task<Paymentmode> GetPaymentDetails(int PaymentId);

    Task updateOrderTable(OrderTable table);

    Task updateTable(Table tables);

    Task<OrderItem> GetorderItemForDelete(int deleteId);
    Task<OrderItem> GetOrderItem(int OrderItemId);

    Task UpdateOrderItem(OrderItem orderItem);

    Task<OrderItem> AddOrderItem(OrderItem orderItem);

    Task<OrderItemModifier> AddOrderItemModifier(OrderItemModifier orderItemModifier);

    Task AddTax( OrderTax orderTax);

    Task<Paymentmode> GetPayment(int PaymentModeId);

    Task UpdateOrderPayment(Paymentmode orderPayemnt);

    Task<Paymentmode> AddPayment(Paymentmode payment);

    Task<List<OrderTable>> GetTables(int Orderid);
    Task<Table> ChangeTableData(int CustomerId);

    Task SaveTableData(Table table);

    Task<bool> CheckReadyQuantity(int orderId);
    Task<bool> CheckReadyQuantityForCancel(int orderId);

    Task<Order> GetOrderDetailsForRating(int OrderId);

    Task<Rating> AddCustomerRatting(Rating rating);


}

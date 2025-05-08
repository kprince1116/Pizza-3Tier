using System.Security.Cryptography;
using BAL.Models.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Pizzashop.DAL.ViewModels;

namespace BAL.Services;

public class OrderAppMenu : IOrderAppMenu
{
    private readonly IOrderAppMenuRepository _orderAppMenuRepository;
    public OrderAppMenu(IOrderAppMenuRepository orderAppMenuRepository)
    {
        _orderAppMenuRepository = orderAppMenuRepository;
    }

    public async Task<OrderAppMenuviewmodel> GetCategories()
    {
        var category = await _orderAppMenuRepository.GetCategories();

        var viewmodel = new OrderAppMenuviewmodel
        {
            categories = category.Select(c => new MenuCategoryviewmodel
            {
                CategoryId = c.Categoryid,
                CategoryName = c.Name
            }).ToList()
        };
        return viewmodel;
    }

    public async Task<OrderAppMenuviewmodel> GetItems(int CategoryId, string SearchKey)
    {
        try
        {
            var items = await _orderAppMenuRepository.GetItems(CategoryId, SearchKey);

            var viewmodel = new OrderAppMenuviewmodel
            {
                items = items.Select(i => new Itemsviewmodel
                {
                    ItemId = i.Itemid,
                    ItemName = i.Itemname,
                    ItemType = i.Itemtype,
                    Image = i.Image,
                    ItemTax =  i.TaxPercentage ?? 0 , 
                    price = (decimal) i.Rate,
                    isFavourite = (bool)i.IsFavourite
                }).ToList()
            };
            return viewmodel;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }

    }

    public async Task<OrderAppMenuviewmodel> GetFavouriteItems(string SearchKey)
    {
        var items = await _orderAppMenuRepository.GetFavouriteItems(SearchKey);

        var viewmodel = new OrderAppMenuviewmodel
        {
            items = items.Select(i => new Itemsviewmodel
            {
                ItemId = i.Itemid,
                ItemName = i.Itemname,
                ItemType = i.Itemtype,
                ItemTax =  i.TaxPercentage ?? 0 ,
                price = (decimal)i.Rate,
                isFavourite = (bool)i.IsFavourite
            }).ToList()
        };
        return viewmodel;
    }

    public async Task<bool> UpdateFavourite(int ItemId)
    {
        var item = await _orderAppMenuRepository.GetItemById(ItemId);

        if (item == null)
        {
            return false;
        }

        if (item.IsFavourite == true)
        {
            item.IsFavourite = false;
        }
        else
        {
            item.IsFavourite = true;
        }

        await _orderAppMenuRepository.UpdateItem(item);

        return true;
    }

    public async Task<MenuItemModalviewmoel> GetModalData(int ItemId)
    {
        var item = await _orderAppMenuRepository.GetItemForModalById(ItemId);

        if (item == null)
        {
            return null;
        }

        var viewmodel = new MenuItemModalviewmoel
        {
            ItemId = item.Itemid,
            ItemName = item.Itemname,
            ItemTax = item.TaxPercentage ?? 0,
            ModifierGroupList = item.Itemmodifiergroups.Select(u => new MenuModifierGroupViewModel
            {
                ModifierGroupId = (int)u.Modifiergroupid,
                ModifierGroupName = u.Modifiergroup.Name,
                Min = (int)u.Minallowed,
                Max = (int)u.Maxallowed,
                ModifierList = u.Modifiergroup.Modifiermappings.Select(v => new MenuModifierViewModel
                {
                    ModifierId = (int)v.Modifier.Modifierid,
                    ModifierName = v.Modifier.Modifiername,
                    Price = (decimal)v.Modifier.Rate
                }).ToList()
            }).ToList()
        };
        return viewmodel;
    }

    public async Task<OrderAppMenuviewmodel> GetOrderData(int OrderId)
    {
        var tableData = await _orderAppMenuRepository.GetTableData(OrderId);

        var itemData = await _orderAppMenuRepository.GetOrderItems(OrderId);

        var taxList = await _orderAppMenuRepository.GetTaxList();

        var taxListfrommapping = await _orderAppMenuRepository.GetTaxLists(OrderId);

        var viewmodel = new OrderAppMenuviewmodel
        {
            OrderId = (int)tableData.Orderid,
            OrderStatus = tableData.StatusNavigation.Status,
            PaymentStatus = tableData.PaymentModeNavigation?.Status,
            CustomerId = (int)tableData.CustomerId,
            tables = tableData.OrderTables.Select(t => new tableviewmodel
            {
                sectionId = (int)t.Table.Section.Sectionid,
                sectionName = t.Table.Section.SectionName,
                TableId = (int)t.TableId,
                TableName = t.Table.TableName
            }).ToList(),

            orderitems = itemData.Select(i => new OrderItemviewmodel
            {
                ItemId = (int)i.ItemId,
                ItemName = i.Item.Itemname,
                OrderItemId = i.Id,
                Readyitem =(int) i.ReadyItem,
                ItemTax =  i.Item.TaxPercentage ?? 0 ,
                price = (decimal)i.Item.Rate,
                Quantity = (int)i.Quantity,
                TotalAmount = (decimal)(i.Item.Rate * i.Quantity),
                modifiers = i.OrderItemModifiers.Select(m => new ModiferListModel
                {
                    ModifierId = m.Modifier.Modifierid,
                    ModifierName = m.Modifier.Modifiername,
                    price = (decimal)m.Modifier.Rate,
                    Quantity = (int)m.Quantity,
                    TotalAmount = (decimal)(m.Modifier.Rate * m.Quantity),
                    
                }).ToList(),
                TotalModifierAmount = (decimal) i.OrderItemModifiers.Sum(oi=>oi.Modifier.Rate * i.Quantity)
              
            }).ToList()        
        };

        if(viewmodel.OrderStatus == "In Progress")
        {
             viewmodel.orderTax = taxListfrommapping.Select(i => new MenuTaxviewmodel{
             TaxId = (int) i.TaxId ,
             TaxName = i.Tax.Taxname ,
             TaxRate = (Decimal)i.TaxFlat ,
            TaxType =(bool) i.Tax.TaxType
           }).ToList();
        }
        else{
             viewmodel.orderTax = taxList.Select(i => new MenuTaxviewmodel{
             TaxId = (int) i.Taxid ,
             TaxName = i.Taxname ,
             TaxRate = (Decimal)i.Taxvalue ,
            TaxType =(bool) i.TaxType
           }).ToList();
        }

        return viewmodel;
    }

    public async Task<CustomerDetailsForOrderViewModel> GetCustomerDetails(int OrderId)
    {
        var customer = await _orderAppMenuRepository.GetCustomerDetails(OrderId);

        if (customer == null)
        {
            return null;
        }

        var viewmodel = new CustomerDetailsForOrderViewModel
        {
            OrderId = (int)customer.Orderid,
            CustomerId = (int)customer.CustomerId,
            CustomerName = customer.Customer.Customername,
            CustomerPhone = customer.Customer.Phonenumber,
            CustomerEmail = customer.Customer.Customeremail,
            NoOfPersons = customer.NoOfPerson ?? 0
        };

        return viewmodel;
    }

    public async Task<bool> SaveCustomerDetails(CustomerDetailsForOrderViewModel model)
    {
        try
        {
            var customer = await _orderAppMenuRepository.GetCustomerById(model.CustomerId);
            var order = await _orderAppMenuRepository.GetOrderDetails(model.OrderId);

            if (customer == null)
            {
                return false;
            }
            customer.Customername = model.CustomerName;
            customer.Phonenumber = model.CustomerPhone;
            order.NoOfPerson = model.NoOfPersons;

            await _orderAppMenuRepository.UpdateCustomer(customer);
            await _orderAppMenuRepository.UpdateOrder(order);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

    }
    public async Task<OrderWiseCommentViewModel> GetOrderComments(int OrderId)
    {
        var order = await _orderAppMenuRepository.GetOrderComments(OrderId);

        var viewmodel = new OrderWiseCommentViewModel
        {
            OrderId = (int)order.Orderid,
            Comment = order.Instruction
        };

        return viewmodel;
    }
    public async Task<Itemwisecommentviewmodel> GetItemComments(int OrderId)
    {
        var order = await _orderAppMenuRepository.GetItemComments(OrderId);

        var viewmodel = new Itemwisecommentviewmodel
        {
            OrderItemId = (int)order.Id,
            Comment = order.OrderItemInstruction
        };

        return viewmodel;
    }

    public async Task<bool> PostComment(OrderWiseCommentViewModel model)
    {
        try
        {
            var order = await _orderAppMenuRepository.GetOrderComments(model.OrderId);

            if (order == null)
            {
                return false;
            }
            order.Instruction = model.Comment;

            await _orderAppMenuRepository.UpdateOrder(order);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

    }
    
    public async Task<bool> PostItemComment(Itemwisecommentviewmodel model)
    {
        try
        {
            var order = await _orderAppMenuRepository.GetItemComments(model.OrderItemId);

            if (order == null)
            {
                return false;
            }
            order.OrderItemInstruction = model.Comment;

            await _orderAppMenuRepository.UpdateOrderItem(order);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

    }

    public async Task<bool> SaveOrder(int OrderId,string OrderStatus, List<OrderItemviewmodel> save_items, List<int> delete_items, List<MenuTaxviewmodel>save_tax,string payment_type)
    {   

        var order = await _orderAppMenuRepository.GetOrderDetails(OrderId);

        for(int i=0; i<delete_items.Count;i++)
        {
            var orderItem = await _orderAppMenuRepository.GetorderItemForDelete(delete_items[i]);
            if(orderItem!=null)
            {
                orderItem.IsDeleted = true;
                orderItem.ModifiedDate = DateTime.Now;
                await _orderAppMenuRepository.UpdateOrderItem(orderItem);
            }
        }

        decimal total = 0;
        decimal subtotal = 0;
        decimal exclusivetax = 0;
        
        for(int i=0;i<save_items.Count;i++)
        {
            decimal modifier_sum = 0 ;

            if(save_items[i].OrderItemId !=0)
            {
                var orderItem = await _orderAppMenuRepository.GetOrderItem(save_items[i].OrderItemId);
                
                if(orderItem.Quantity != save_items[i].Quantity)
                {
                    orderItem.Quantity = save_items[i].Quantity;
                    orderItem.Status = "In Progress";
                    orderItem.ModifiedDate = DateTime.Now;
                    await _orderAppMenuRepository.UpdateOrderItem(orderItem);
                }
                if(orderItem.OrderItemModifiers.Count != 0){
                    modifier_sum +=(decimal) orderItem.OrderItemModifiers.Sum(u=>u.Price);
                }
            }
            else{
                OrderItem orderItem = new(){
                    OrderId = OrderId,
                    ItemId = save_items[i].ItemId,
                    Quantity = save_items[i].Quantity,
                    Price = save_items[i].price,
                    ItemTaxPercenetage = save_items[i].ItemTax,
                    Status = "In Progress",
                    ReadyItem = 0,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now
                };
                orderItem = await _orderAppMenuRepository.AddOrderItem(orderItem);

                for(int j=0;j<save_items[i].modifiers.Count;j++)
                {
                    OrderItemModifier orderItemModifier = new(){
                        OrderItemId = orderItem.Id,
                        ModifierId = save_items[i].modifiers[j].ModifierId,
                        Quantity = save_items[i].Quantity,
                        Price = save_items[i].modifiers[j].price
                    };
                    await _orderAppMenuRepository.AddOrderItemModifier(orderItemModifier);
                    modifier_sum += (decimal) save_items[i].modifiers[j].price;
                }
            }
            decimal item_sum = save_items[i].price * save_items[i].Quantity ;
            subtotal += item_sum + (modifier_sum*save_items[i].Quantity);
            exclusivetax += item_sum * save_items[i].ItemTax/100;
        }

        decimal TotalTax = 0;
        for(int i=0 ; i<save_tax.Count ; i++)
        {
            if(order.StatusNavigation.Status == "Pending")
            {
                OrderTax orderTax = new(){
                    OrderId = OrderId,
                    TaxId = save_tax[i].TaxId,
                    TaxFlat = save_tax[i].TaxRate,
                    TaxType = save_tax[i].TaxType
                };
                await _orderAppMenuRepository.AddTax(orderTax);
            }
            if(save_tax[i].TaxType == true){
                TotalTax += subtotal * save_tax[i].TaxRate/100;
            }
            else{
                 TotalTax += save_tax[i].TaxRate;
            }
        }
        total = subtotal + TotalTax + exclusivetax; 

        if(order.PaymentMode != null)
        {
            Paymentmode orderPayemnt = await _orderAppMenuRepository.GetPayment((int)order.PaymentMode);
            orderPayemnt.Totalamount = total;
            orderPayemnt.Status = payment_type;
            orderPayemnt.Totalamount = total;
            await _orderAppMenuRepository.UpdateOrderPayment(orderPayemnt);
        }
        else{
            Paymentmode orderPayment = new(){
                Totalamount = total,
                Status = payment_type,
            };
            orderPayment = await _orderAppMenuRepository.AddPayment(orderPayment);
            order.PaymentMode = orderPayment.Id;
        }
        await _orderAppMenuRepository.UpdateOrder(order);

        if(order.StatusNavigation.Status == "Pending")
        {
            var table = await _orderAppMenuRepository.ChangeTableData((int)order.CustomerId);
            if(table != null)
            {
                table.Status = "Running";
                table.ModifiedDate = DateTime.Now;
                await _orderAppMenuRepository.SaveTableData(table);
            }
            order.Status = 4;
            order.ModifiedDate = DateTime.Now;
            order.TotalAmount = total;
            await _orderAppMenuRepository.UpdateOrder(order);
        }
         else
         {
         order.TotalAmount = total;
         order.ModifiedDate = DateTime.Now;
         await _orderAppMenuRepository.UpdateOrder(order);
         }

        return true;
    }

    public async Task<bool> CheckReadyQuantity(int orderId)
    {
        var check = await _orderAppMenuRepository.CheckReadyQuantity(orderId);
        return check;
    }
    public async Task<bool> CheckReadyQuantityForCancel(int orderId)
    {
        var check = await _orderAppMenuRepository.CheckReadyQuantityForCancel(orderId);
        return check;
    }

    public async Task<bool> CompleteOrder(int orderId)
    {
        var order = await _orderAppMenuRepository.GetOrderDetails(orderId);

        if(order == null)
        {
            return false;
        }

        foreach(var table in order.OrderTables){
            table.IsDeleted = true;
            await _orderAppMenuRepository.updateOrderTable(table);

            var tables = await _orderAppMenuRepository.GetTable((int)table.TableId);
            tables.Isavailable = true;
            tables.Status = "Available";
            tables.ModifiedDate = DateTime.Now;
            tables.CustomerId = null;
            await _orderAppMenuRepository.updateTable(tables);
        }

        var orderPayment = await _orderAppMenuRepository.GetPaymentDetails((int)order.PaymentMode);

        orderPayment.PaidOn = DateTime.Now;
        orderPayment.PaymentStatus = "paid";

        await _orderAppMenuRepository.UpdateOrderPayment(orderPayment);

        order.Status = 2;
        order.ModifiedDate = DateTime.Now;

        await _orderAppMenuRepository.UpdateOrder(order);

        return true;

    }
    public async Task<bool> CancelOrder(int orderId)
    {
        var order = await _orderAppMenuRepository.GetOrderDetails(orderId);

        if(order == null)
        {
            return false;
        }

        foreach(var table in order.OrderTables){
            table.IsDeleted = true;
            await _orderAppMenuRepository.updateOrderTable(table);

            var tables = await _orderAppMenuRepository.GetTable((int)table.TableId);
            tables.Isavailable = true;
            tables.Status = "Available";
            tables.ModifiedDate = DateTime.Now;
            tables.CustomerId = null;
            await _orderAppMenuRepository.updateTable(tables);
        }

        var orderPayment = await _orderAppMenuRepository.GetPaymentDetails((int)order.PaymentMode);

        orderPayment.PaidOn = DateTime.Now;
        orderPayment.PaymentStatus = "canceled";

        await _orderAppMenuRepository.UpdateOrderPayment(orderPayment);

        order.Status = 3;
        order.ModifiedDate = DateTime.Now;

        await _orderAppMenuRepository.UpdateOrder(order);

        return true;

    }

    public async Task<bool> AddCustomerRatting(OrderAppMenuviewmodel model)
    {
        
        var order = await _orderAppMenuRepository.GetOrderDetailsForRating(model.OrderId);

        if(order == null){
            return false;
        }
        
        try
        {
            Rating rating1 = new(){
                Ambiencerating = model.AmbienceRating,
                Foodrating = model.FoodRating,
                Servicerating = model.ServiceRating,
                Comments = model.comments
            };

        var rating = await _orderAppMenuRepository.AddCustomerRatting(rating1);

        order.Rating = rating.Ratingid;

        await _orderAppMenuRepository.UpdateOrder(order);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
     
      

        return true;

    }

}

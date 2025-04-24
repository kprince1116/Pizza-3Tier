using BAL.Models.Interfaces;
using DAL.Interfaces;
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
                    price = (int)i.Rate,
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
                price = (int)i.Rate,
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

        var viewmodel = new OrderAppMenuviewmodel
        {
            OrderId = (int)tableData.Orderid,
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
                price = (decimal)i.Item.Rate,
                Quantity = (int)i.Quantity,
                TotalAmount = (decimal)(i.Item.Rate * i.Quantity),
                modifiers = i.OrderItemModifiers.Select(m => new ModiferListModel
                {
                    ModifierId = m.Modifier.Modifierid,
                    ModifierName = m.Modifier.Modifiername,
                    price = (decimal)m.Modifier.Rate,
                    Quantity = (int)m.Quantity,
                    TotalAmount = (decimal)(m.Modifier.Rate * m.Quantity)
                }).ToList()

            }).ToList() ,

           orderTax = taxList.Select(i => new MenuTaxviewmodel{
             TaxId = (int) i.Taxid ,
             TaxName = i.Taxname ,
             TaxRate = (Decimal)i.Taxvalue ,
            TaxType =(bool) i.TaxType
           }).ToList()

        };
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

}

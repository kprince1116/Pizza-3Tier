using DAL.Models;
using Microsoft.Identity.Client;

namespace Pizzashop.DAL.ViewModels;

public class Kotviewmodel
{
    public List<MenuCategory> Categories { get; set; }
    public List<OrderDetailsViewModel> OrderDetails { get; set; } = new List<OrderDetailsViewModel>();
    public class OrderDetailsViewModel
    {
        public int orderId { get; set; }
        public int OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public List<Table> TableNo { get; set; }
        public string SectionName { get; set; }
        public string OrderStatus { get; set; }
        public string OrderInstruction { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
    }

    public class OrderItemViewModel
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public int PendigQuantity { get; set; }
        public int ReadyQuantity { get; set; }
        public string ItemInstrucion { get; set; }
        public int CategoryId { get; set; }
        public List<ModifierViewModel> Modifiers { get; set; } = new List<ModifierViewModel>();
    }

    public class ModifierViewModel
    {
        public string ModifierName { get; set; }
    }
}

public class KOTVM
{
    public int CategoryId {get; set;}
    public List<GetKotDataViewModel> Getkotdata { get; set; } = new List<GetKotDataViewModel>();

    public class GetKotDataViewModel
    {
        public int OrderId { get; set; }
        public int OrderNo { get; set; }
        public string TableName { get; set; }
        public string SectionName { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderInstruction { get; set; }
        public string OrderStatus { get; set; }
        public List<OrderItemViewModels> ItemLists { get; set; }

        public class OrderItemViewModels
        {
            public string ItemName { get; set; }
            public int Quantity { get; set; }
            public int PendigQuantity { get; set; }
            public int ReadyQuantity { get; set; }
            public string ItemInstrucion { get; set; }
            public int CategoryId { get; set; }
            public List<ModifierViewModels> Modifiers { get; set; } = new List<ModifierViewModels>();
        }

        public class ModifierViewModels
        {
            public string ModifierName { get; set; }
        }


    }
}

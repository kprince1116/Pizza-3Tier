using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Order
{
    public int Orderid { get; set; }

    public int OrderNo { get; set; }

    public DateTime? Orderdate { get; set; }

    public int? CustomerId { get; set; }

    public int? Status { get; set; }

    public int? PaymentMode { get; set; }

    public string? Instruction { get; set; }

    public decimal? TotalAmount { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? Isdelete { get; set; }

    public string? InvoiceNo { get; set; }

    public string? OrdereType { get; set; }

    public int? NoOfPerson { get; set; }

    public int? Rating { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<OrderTable> OrderTables { get; set; } = new List<OrderTable>();

    public virtual ICollection<OrderTax> OrderTaxes { get; set; } = new List<OrderTax>();

    public virtual Paymentmode? PaymentModeNavigation { get; set; }

    public virtual Rating? RatingNavigation { get; set; }

    public virtual Orderstatus? StatusNavigation { get; set; }
}

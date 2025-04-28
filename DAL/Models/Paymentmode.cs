using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Paymentmode
{
    public int Id { get; set; }

    public decimal? Totalamount { get; set; }

    public DateTime? PaidOn { get; set; }

    public string? Status { get; set; }

    public string? PaymentStatus { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

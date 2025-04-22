using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class OrderTable
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int? TableId { get; set; }

    public int? CustomerId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Table? Table { get; set; }
}

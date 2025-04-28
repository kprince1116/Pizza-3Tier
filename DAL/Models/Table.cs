using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Table
{
    public int Tableid { get; set; }

    public string TableName { get; set; } = null!;

    public int? Sectionid { get; set; }

    public int Capacity { get; set; }

    public bool? Isavailable { get; set; }

    public bool? Isdeleted { get; set; }

    public long? CreatedBy { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? Status { get; set; }

    public int? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderTable> OrderTables { get; set; } = new List<OrderTable>();

    public virtual Section? Section { get; set; }
}

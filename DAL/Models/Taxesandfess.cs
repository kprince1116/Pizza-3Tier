using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Taxesandfess
{
    public int Taxid { get; set; }

    public string Taxname { get; set; } = null!;

    public bool? TaxType { get; set; }

    public decimal? FlatAmount { get; set; }

    public decimal? Percentage { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdefault { get; set; }

    public decimal? Taxvalue { get; set; }

    public bool? Isdeleted { get; set; }

    public long? CreatedBy { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<OrderTax> OrderTaxes { get; set; } = new List<OrderTax>();
}

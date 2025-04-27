﻿using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class OrderTax
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int? TaxId { get; set; }

    public decimal? TaxPercentage { get; set; }

    public decimal? TaxFlat { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Taxesandfess? Tax { get; set; }
}

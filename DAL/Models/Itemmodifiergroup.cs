using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Itemmodifiergroup
{
    public int Id { get; set; }

    public int? Itemid { get; set; }

    public int? Modifiergroupid { get; set; }

    public int? Maxallowed { get; set; }

    public int? Minallowed { get; set; }

    public bool? IsDeleted { get; set; }

    public long? CreatedBy { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual MenuItem? Item { get; set; }

    public virtual ModifierGroup? Modifiergroup { get; set; }
}

using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Modifiermapping
{
    public int Id { get; set; }

    public int? ModifierGroupId { get; set; }

    public int? ModifierId { get; set; }

    public bool? IsDeleted { get; set; }

    public long? CreatedBy { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Modifier? Modifier { get; set; }

    public virtual ModifierGroup? ModifierGroup { get; set; }
}

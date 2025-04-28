using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class ModifierGroup
{
    public int ModifierGroupId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsDeleted { get; set; }

    public long? CreatedBy { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<Itemmodifiergroup> Itemmodifiergroups { get; set; } = new List<Itemmodifiergroup>();

    public virtual ICollection<Modifiermapping> Modifiermappings { get; set; } = new List<Modifiermapping>();

    public virtual ICollection<Modifier> Modifiers { get; set; } = new List<Modifier>();
}

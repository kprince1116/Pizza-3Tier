using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Section
{
    public int Sectionid { get; set; }

    public string SectionName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool? Isdeleted { get; set; }

    public long? CreatedBy { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<Table> Tables { get; set; } = new List<Table>();
}

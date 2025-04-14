using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class WaitingToken
{
    public int Id { get; set; }

    public int? Customerid { get; set; }

    public int? NoOfPersons { get; set; }

    public int? SectionId { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsAssigned { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? AssignedTime { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual Section? Section { get; set; }
}

using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Rolesandpermission
{
    public int Rolesandpermissionid { get; set; }

    public int? Userroleid { get; set; }

    public int? Permissionid { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool CanView { get; set; }

    public bool CanAddEdit { get; set; }

    public bool CanDelete { get; set; }

    public long? CreatedBy { get; set; }

    public long? ModifiedBy { get; set; }

    public bool? Isdeleted { get; set; }

    public virtual Permission? Permission { get; set; }

    public virtual Userrole1? Userrole { get; set; }
}

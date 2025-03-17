using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Permission
{
    public int Permissionid { get; set; }

    public string PermissionName { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public long? CreatedBy { get; set; }

    public long? ModifiedBy { get; set; }

    public bool? Isdeleted { get; set; }

    public virtual ICollection<Rolesandpermission> Rolesandpermissions { get; set; } = new List<Rolesandpermission>();
}

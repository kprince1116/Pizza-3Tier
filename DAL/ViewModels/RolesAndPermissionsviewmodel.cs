namespace DAL.ViewModels;

public class RolesAndPermissionsviewmodel
{
    public int Rolesandpermissionid { get; set; }

    public int? Userroleid { get; set; }

    public int? Permissionid { get; set; }

    public string RoleName { get; set; }

    public string PermissionName { get; set; }

    public bool IsSelected {get ; set;}

    public bool CanView { get; set; }

    public bool CanAddEdit { get; set; }

    public bool CanDelete { get; set; }
 
}

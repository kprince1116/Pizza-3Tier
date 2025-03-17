using BAL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
using Pizzashop.DAL.ViewModels;

namespace BAL.Services;

public class UserRolesAndPermissions : IUserRolesAndPermissions
{

    private readonly IUserRolesAndPermissionsRepository _userRolesAndPermissionsRepository;

    public UserRolesAndPermissions(IUserRolesAndPermissionsRepository userRolesAndPermissionsRepository)
    {
        _userRolesAndPermissionsRepository = userRolesAndPermissionsRepository;
    }

    public List<Userrole1> GetRoles()
    {
        return _userRolesAndPermissionsRepository.GetRoles();
    }

    public async Task<List<RolesAndPermissionsviewmodel>> GetPermissions(int id)
    {
        var permission = await _userRolesAndPermissionsRepository.GetPermissions(id);
        List<RolesAndPermissionsviewmodel> role_permission_list = new List<RolesAndPermissionsviewmodel>();
        foreach (var data in permission)
        {
            RolesAndPermissionsviewmodel role_permission = new RolesAndPermissionsviewmodel();
            role_permission.Rolesandpermissionid = data.Rolesandpermissionid;
            role_permission.Userroleid = data.Userroleid;
            role_permission.Permissionid = data.Permissionid;
            role_permission.RoleName = data.Userrole.RoleName;
            role_permission.PermissionName = data.Permission.PermissionName;
            role_permission.CanView = data.CanView;
            role_permission.CanAddEdit = data.CanAddEdit;
            role_permission.CanDelete = data.CanDelete;
            role_permission_list.Add(role_permission);
        }
        return role_permission_list;
    }


    public async Task<List<RolesAndPermissionsviewmodel>> UpdatePermissions(List<RolesAndPermissionsviewmodel> user)
    {
         var updatedPermissions = new List<RolesAndPermissionsviewmodel>();

        foreach (var item in user)
        {
             try
        {
            var permissions = await _userRolesAndPermissionsRepository.GetPermissionsById(item.Rolesandpermissionid);

            foreach (var permission in permissions)
            {
                permission.CanView = item.CanView;
                permission.CanAddEdit = item.CanAddEdit;
                permission.CanDelete = item.CanDelete;

                await _userRolesAndPermissionsRepository.UpdatePermissionsasync(permission);
            }

            updatedPermissions.Add(item);
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"Error updating permissions for Rolesandpermissionid {item.Rolesandpermissionid}: {ex.Message}");
           
        }
        }
        return user;
    }

}

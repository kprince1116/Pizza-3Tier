using DAL.Models;
using DAL.ViewModels;

namespace DAL.Interfaces;

public interface IUserRolesAndPermissionsRepository
{
    List<Userrole1> GetRoles();

    public Task<List<Rolesandpermission>> GetPermissions( int id);

    public Task<List<Rolesandpermission>> GetPermissionsById(int id);

    Task UpdatePermissionsasync(Rolesandpermission Permission);

}

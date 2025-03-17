using DAL.Models;
using DAL.ViewModels;

namespace BAL.Interfaces;

public interface IUserRolesAndPermissions
{

    public List<Userrole1> GetRoles();
    public Task<List<RolesAndPermissionsviewmodel>> GetPermissions(int id);
    public Task<List<RolesAndPermissionsviewmodel>> UpdatePermissions( List<RolesAndPermissionsviewmodel> user);
}

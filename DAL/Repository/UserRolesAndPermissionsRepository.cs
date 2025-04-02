using DAL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class UserRolesAndPermissionsRepository : IUserRolesAndPermissionsRepository
{
     private readonly PizzaShopContext _db;

   
    public UserRolesAndPermissionsRepository(PizzaShopContext db)
        {
            _db = db;
        }

    public List<Userrole1> GetRoles()
    {
       return _db.Userroles1.ToList();
    }

    public async Task<List<Rolesandpermission>> GetPermissions( int id)
    {
        var Permissions = await _db.Rolesandpermissions.Include(r=>r.Userrole).Include(p=>p.Permission).Where(r=>r.Userroleid==id).OrderBy(u=>u.Userroleid).ToListAsync();
        return Permissions;
    }

    public async Task<List<Rolesandpermission>> GetPermissionsById(int id)
    {
        return await _db.Rolesandpermissions.Where(U=>U.Rolesandpermissionid == id).ToListAsync();
    }

    public async Task UpdatePermissionsasync(Rolesandpermission Permission)
    {
         _db.Rolesandpermissions.Update(Permission);
         await _db.SaveChangesAsync();
    }

    
   
}

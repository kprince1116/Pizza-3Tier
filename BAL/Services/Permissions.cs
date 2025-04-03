using System.Security.Claims;
using BAL.Interfaces;
using BAL.Models.Interfaces;
using Pizzashop.DAL.ViewModels;
using Microsoft.AspNetCore.Http;
using DAL.Models;
using Microsoft.EntityFrameworkCore;


namespace BAL.Services
{
    public class Permissions : IPermissions
    {
        private readonly PizzaShopContext _db;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor; 

        public Permissions(ITokenService tokenService, IHttpContextAccessor httpContextAccessor , PizzaShopContext db)
        {
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor; 
            _db=db;
        }

        public async Task<bool> HasPermission(string module, ActionPermissions action)
        {
            try
            {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["jwtToken"];
            var userId = await _tokenService.GetIdFromToken(token);

            if(userId==null)
            {
               Console.WriteLine("User ID is null.");
               return false;
            }

            var userIdValue =  userId;
            var user = _db.Users.FirstOrDefault(u => u.UserId == userIdValue);
            if (user == null)
             {
                 Console.WriteLine("User or User role is null.");
                  return false; 
             }

            var roleid = user.Userrole;

            var permissions = await  _db.Permissions.FirstOrDefaultAsync(P=>P.PermissionName.ToLower() == module.ToLower());      

            if (permissions == null)
            {
                Console.WriteLine("Permissionid is null.");
                return false; 
            }
            else{
                var PermissionId = permissions.Permissionid;
                var permission = _db.Rolesandpermissions.FirstOrDefault(p=>p.Userroleid == roleid && p.Permissionid == PermissionId);
                 if (permission == null)
             {
                Console.WriteLine("Permission mapping is null.");
                return false; 
             }

            Console.WriteLine($"User ID: {userIdValue}, Role ID: {roleid}, Permission ID: {PermissionId}");


            if(permission == null)
            {
               return false;
            }

            return action switch
            {
               ActionPermissions.CanView => permission.CanView,
               ActionPermissions.CanAddEdit => permission.CanAddEdit,
               ActionPermissions.CanDelete => permission.CanDelete,
               _ => false
            }; 
            }
           
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception",e.Message);
                return false;
            }

    }
}
}
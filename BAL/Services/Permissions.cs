using System.Security.Claims;
using BAL.Interfaces;
using BAL.Models.Interfaces;
using Pizzashop.DAL.ViewModels;
using Microsoft.AspNetCore.Http;
using DAL.Models;


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
    
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["jwtToken"];
            var userId = _tokenService.GetIdFromToken(token);

            if(userId==null)
            {
               return false;
            }

            var userIdValue = await userId;
            var roleid = _db.Users.FirstOrDefault(u => u.UserId == userIdValue).UserroleNavigation.Roleid;
            var permissionid = _db.Permissions.FirstOrDefault(P=>P.PermissionName.ToLower() == module.ToLower()).Permissionid;

            var permission = _db.Rolesandpermissions.FirstOrDefault(p=>p.Userroleid == roleid && p.Permissionid == permissionid);

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
}
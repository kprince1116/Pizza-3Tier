using System.Security.Claims;
using BAL.Interfaces;
using BAL.Models.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BAL.Services;

public class UserRoleService : IUserroleService
{

         private readonly PizzaShopContext _db;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor; 

        public UserRoleService(ITokenService tokenService, IHttpContextAccessor httpContextAccessor , PizzaShopContext db)
        {
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor; 
            _db=db;
        }


    public async Task<bool> IsUserInRoleAsync(string roleName)
    {

        var token = _httpContextAccessor.HttpContext?.Request.Cookies["jwtToken"];
        var userId = await _tokenService.GetIdFromToken(token);

        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);

        var roleid = user.Userrole;

        var rolename = await _db.Userroles1.FirstOrDefaultAsync(r => r.Userroleid == roleid);

        if(rolename.RoleName == "Account_Manager")
        {
            return true;
        }
        return false;
    }


}

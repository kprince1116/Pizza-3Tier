using System.Security.Claims;

namespace BAL.Models.Interfaces;

public interface IUserroleService
{
    Task<bool> IsUserInRoleAsync(string roleName);
}

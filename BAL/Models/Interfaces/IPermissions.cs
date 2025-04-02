using System.Security.Claims;
using Pizzashop.DAL.ViewModels;

namespace BAL.Models.Interfaces;

public interface IPermissions
{
     public Task<bool> HasPermission(string module , ActionPermissions action);
}    

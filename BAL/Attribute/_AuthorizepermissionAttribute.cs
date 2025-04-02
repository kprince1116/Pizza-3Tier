using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pizzashop.DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace BAL.Attributes;
public class _AuthPermissionAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _module;
    private readonly ActionPermissions _action;

    public _AuthPermissionAttribute(string module, ActionPermissions action)
    {
        _module = module;
        _action = action;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // Resolve the required services
        var permissionService = context.HttpContext.RequestServices.GetService<IAuthorizationSer>();

        // Check if the user has the required permission
        var hasPermission = await permissionService.HasPermission(_module, _action);
        if (!hasPermission)
        {
            
        }
    }
}

\
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pizzashop.DAL.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using BAL.Models.Interfaces;

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
        var permissionService = context.HttpContext.RequestServices.GetRequiredService<IPermissions>();

        var hasPermission = await permissionService.HasPermission(_module, _action);
        if (!hasPermission)
        {
            context.Result = new RedirectToRouteResult(new RouteValueDictionary
            {
                { "controller", "Auth" }, 
                { "action", "AccessDenied" } 
            });
        }
    }
}


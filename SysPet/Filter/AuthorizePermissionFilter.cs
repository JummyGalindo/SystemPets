using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AuthorizePermissionFilter : IAuthorizationFilter
{
    public string RequiredRole { get; set; }

    public AuthorizePermissionFilter(string requiredRole)
    {
        RequiredRole = requiredRole;
    }


    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var roles = new List<string> { "Administrador", "Usuario" };
        var user = context.HttpContext.User;
        
        if (roles.Contains(RequiredRole) && !user.Claims.Any())
        {
            context.Result = new RedirectToActionResult("LogIn", "User", null);
        }
        if (roles.Contains(RequiredRole) && user.HasClaim(c => c.Type == ClaimTypes.Role))
        {

        }
    }
}

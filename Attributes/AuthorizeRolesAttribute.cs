using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PhotoForum.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthorizeRolesAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] allowedRoles;

        public AuthorizeRolesAttribute(params string[] roles)
        {
            allowedRoles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Retrieve the JWT token from the "Authorization" cookie
            var token = context.HttpContext.Request.Cookies["Authorization"];

            if (string.IsNullOrEmpty(token))
            {
                // Redirect to the login page if the token is missing
                context.Result = new RedirectToRouteResult(new { controller = "Home", action = "Index" });
                return;
            }

            // Perform token validation and decoding (using a JWT library like System.IdentityModel.Tokens.Jwt)
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            // Retrieve the roles claim from the token
            var rolesClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Check if the user has any of the allowed roles
            if (!IsAuthorized(rolesClaim))
            {
                context.Result = new RedirectToRouteResult(new { controller = "Home", action = "Index" });
            }
        }

        private bool IsAuthorized(string rolesClaim)
        {
            // Check if the user has any of the allowed roles based on the roles claim
            return !string.IsNullOrEmpty(rolesClaim) && allowedRoles.Any(role => role == rolesClaim);
        }
    }
}

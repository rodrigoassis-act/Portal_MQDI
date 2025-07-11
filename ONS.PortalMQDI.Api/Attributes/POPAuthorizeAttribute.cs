using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ONS.PortalMQDI.Models.Enum;
using ONS.PortalMQDI.Models.Response;
using ONS.PortalMQDI.Services.Services;
using System;
using System.Net;

namespace ONS.PortalMQDI.Api.Attributes
{
    public class POPAuthorizeAttribute : TypeFilterAttribute
    {
        public PermissionEnum[] Permissions { get; set; }

        public POPAuthorizeAttribute(params PermissionEnum[] permissions) : base(typeof(ClaimRequirementFilter))
        {
            this.Permissions = permissions;
            Arguments = new object[] { permissions };
        }
    }

    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly PermissionEnum[] _permissions;
        readonly JwtService _jwtService;
        public ClaimRequirementFilter(PermissionEnum[] permissions, JwtService jwtService)
        {
            _permissions = permissions;
            _jwtService = jwtService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            try
            {
                if (!_jwtService.CheckPermission(_permissions))
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new JsonResult(new PortalMQDIResponse(HttpStatusCode.Unauthorized));
                }
            }
            catch (Exception ex)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new JsonResult(new PortalMQDIResponse(HttpStatusCode.Unauthorized, ex.Message));
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ONS.PortalMQDI.Api.Attributes;
using ONS.PortalMQDI.Models.Enum;
using ONS.PortalMQDI.Services.Services;
using ONS.PortalMQDI.Shared.Extensions;
using System.Diagnostics;
using System.Linq;

namespace ONS.PortalMQDI.Api.Controllers
{
    [POPAuthorize(PermissionEnum.PortalMQDI)]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly JwtService _jwtService;
        public Stopwatch stopwatch;

        public BaseController(JwtService jwtService)
        {
            _jwtService = jwtService;
            stopwatch = new Stopwatch();
        }

        protected string LoginUsuario
        {
            get
            {
                var usuario = _jwtService.GetClaimFromJwt().FirstOrDefault(c => c.Type == ClaimsEnum.Nameidentifier.GetDescription());
                return usuario != null ? usuario.Value : string.Empty;
            }
        }
    }
}

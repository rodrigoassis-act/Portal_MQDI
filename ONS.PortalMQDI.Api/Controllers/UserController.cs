using Microsoft.AspNetCore.Mvc;
using ONS.PortalMQDI.Models.Response;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Services.Services;
using System.Net;
using System.Threading.Tasks;


namespace ONS.PortalMQDI.Api.Controllers
{
    public class UserController : BaseController
    {

        private readonly IUserService _userService;
        public UserController(JwtService jwtService, IUserService userService) : base(jwtService)
        {
            _userService = userService;
        }


        [HttpGet]
        public async Task<ActionResult<PortalMQDIResponse>> ObterUsuario()
        {
            return Ok(new PortalMQDIResponse(HttpStatusCode.OK, LoginUsuario));
        }

        [HttpGet("VerificaSeUsuarioPodeEditarParametrosSistema")]
        public bool PodeEditarParametrosSistema()
        {
            return _userService.PodeEditarParametrosSistema();
        }
    }
}

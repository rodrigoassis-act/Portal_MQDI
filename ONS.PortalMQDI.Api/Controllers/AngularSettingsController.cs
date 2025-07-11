using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using ONS.PortalMQDI.Shared.Menu;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ONS.PortalMQDI.Models.Response;
using ONS.PortalMQDI.Shared.Settings;
using ONS.PortalMQDI.Services.Interfaces;

namespace ONS.PortalMQDI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AngularSettingsController : ControllerBase
    {
        private readonly IOptions<ServiceGlobalSettings> _serviceGlobalSettings;
        private readonly IOptions<PopServiceSettings> _popServiceSettings;
        private readonly IOptions<ConfigSettings> _configSettings;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _env;

        public AngularSettingsController(IOptions<ServiceGlobalSettings> serviceGlobalSettings,
            IUserService userService,
            IOptions<PopServiceSettings> popServiceSettings,
            IOptions<ConfigSettings> configSettings,
            IWebHostEnvironment env)
        {
            _serviceGlobalSettings = serviceGlobalSettings;
            _userService = userService;
            _popServiceSettings = popServiceSettings;
            _configSettings = configSettings;
            _env = env;
        }

        [HttpGet]
        public ActionResult<AngularSettingsResponse> ObterInformacao()
        {
            try
            {
                var response = new AngularSettingsResponse
                {
                    PopService = new PopServiceResponse
                    {
                        ClientId = _popServiceSettings.Value.ClientId,
                        GrantType = _popServiceSettings.Value.GrantType,
                        Uri = _popServiceSettings.Value.Uri,
                        Origin = _env.IsDevelopment() ? _popServiceSettings.Value.Origin : null ,
                        Username = _env.IsDevelopment() ? _popServiceSettings.Value.Username : null,
                        Password = _env.IsDevelopment() ? _popServiceSettings.Value.Password : null,
                    },
                    Config = new ConfigResponse
                    {
                        Version = _configSettings.Value.Version
                    },
                    MensagemAviso = new MensagemAvisoResponse
                    {
                        Mensagem = _serviceGlobalSettings.Value.MensagemAviso
                    }
                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message}"));
            }
        }

        [HttpGet("menu")]
        public ActionResult<AngularSettingsResponse> ObterMenu()
        {
            try
            {
                var response = new AngularSettingsResponse
                {
                    Menu = MenuHelper.GetSiteMap(_userService.ListaOperacao()),
                    MenuCDN = _serviceGlobalSettings.Value.MenuPopCdn,
                    Permisao = new PermisaoResponse
                    {
                        IsAdministratorOns = _userService.IsAdministrator(),
                        IsAgente = !_userService.IsAdministrator()
                    }
                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message}"));
            }
        }
    }
}

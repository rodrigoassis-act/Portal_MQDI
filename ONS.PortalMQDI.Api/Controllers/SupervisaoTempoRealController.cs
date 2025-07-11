using log4net;
using Microsoft.AspNetCore.Mvc;
using ONS.PortalMQDI.Models.Response;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Services.Services;
using ONS.PortalMQDI.Shared.Extensions;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Api.Controllers
{
    public class SupervisaoTempoRealController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SupervisaoTempoRealController));
        private readonly ISupervisaoTempoRealService _supervisaoTempoRealService;

        public SupervisaoTempoRealController(JwtService jwtService, ISupervisaoTempoRealService supervisaoTempoRealService) : base(jwtService)
        {
            _supervisaoTempoRealService = supervisaoTempoRealService;
        }

        [HttpPost("Buscar")]
        public async Task<ActionResult<PortalMQDIResponse>> BuscarAsync([FromBody] SupervisaoTempoRealFiltroViewModel request, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _supervisaoTempoRealService.BuscarAsync(request, cancellationToken)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpPost("ListaScada")]
        public async Task<ActionResult<PortalMQDIResponse>> ListaScadaAsync([FromBody] SupervisaoTempoRealFiltroViewModel request, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _supervisaoTempoRealService.ListaScadaAsync(request, cancellationToken)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpGet("buscarDatas")]
        public ActionResult<PortalMQDIResponse> BuscarDatas()
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, _supervisaoTempoRealService.BuscarDatas()));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }
    }
}

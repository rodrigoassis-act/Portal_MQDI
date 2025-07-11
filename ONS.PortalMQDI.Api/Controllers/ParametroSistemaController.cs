using log4net;
using Microsoft.AspNetCore.Mvc;
using ONS.PortalMQDI.Models.Response;
using ONS.PortalMQDI.Models.ViewModel;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Services.Services;
using ONS.PortalMQDI.Shared.Extensions;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Api.Controllers
{
    public class ParametroSistemaController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ParametroSistemaController));
        private readonly IParametroSistemaService _parametroSistemaService;
        public ParametroSistemaController(JwtService jwtService, IParametroSistemaService parametroSistemaService) : base(jwtService)
        {
            _parametroSistemaService = parametroSistemaService;
        }

        [HttpGet("BuscaValorLimiteDeIndicadores")]
        public async Task<ActionResult<PortalMQDIResponse>> BuscaValorLimiteDeIndicadoresAsync(CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _parametroSistemaService.BuscaValorLimiteDeIndicadoresAsync(cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpGet("BuscaValorParametroSistema")]
        public async Task<ActionResult<PortalMQDIResponse>> BuscaValorParametroSistemaAsync(CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _parametroSistemaService.BuscaValorParametroSistemaAsync(cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpPost("AdicionarNovoParametro")]
        public async Task<ActionResult<PortalMQDIResponse>> AdicionarNovoParametroAsync([FromBody] ParametroSistemaViewModel param, CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _parametroSistemaService.AdicionarNovoParametroAsync(param, cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpGet("ExisteRegistroFaltanteNoCalendario")]
        public async Task<ActionResult<PortalMQDIResponse>> ExisteRegistroFaltanteNoCalendarioAsync(CancellationToken cancellationToken)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _parametroSistemaService.ExisteRegistroFaltanteNoCalendarioAsync("", cancellationToken)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpGet("AdicionaRegistroFaltanteNoCalendario")]
        public async Task<ActionResult<PortalMQDIResponse>> AdicionaRegistroFaltanteNoCalendarioAsync(string anoMes, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _parametroSistemaService.AdicionaRegistroFaltanteNoCalendarioAsync(anoMes, cancellationToken)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpGet("LimpezaCompleta")]
        public async Task<ActionResult<PortalMQDIResponse>> LimpezaCompletaAsync(string anoMes, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _parametroSistemaService.LimpezaCompletaAsync(anoMes, cancellationToken)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }
    }
}
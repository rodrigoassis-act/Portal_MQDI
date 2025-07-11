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
    public class ResultadoIndicadorController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ResultadoIndicadorController));
        private readonly IResultadoIndicadorService _resultadoIndicadorService;
        public ResultadoIndicadorController(JwtService jwtService, IResultadoIndicadorService resultadoIndicadorService) : base(jwtService)
        {
            _resultadoIndicadorService = resultadoIndicadorService;
        }

        [HttpGet("ListaTpIndicador")]
        public async Task<ActionResult<PortalMQDIResponse>> ListaTpIndicadorAsync(CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _resultadoIndicadorService.ListaTpIndicadorAsync(cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpGet]
        public async Task<ActionResult<PortalMQDIResponse>> FiltroAsync([FromQuery] AcompanhamentoIndicadoresFiltroViewModel filtro, CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _resultadoIndicadorService.FiltroAsync(filtro, cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }


        [HttpGet("sgi/{idResultadoIndicador}")]
        public async Task<ActionResult<PortalMQDIResponse>> BuscarSgiAsync(int idResultadoIndicador, CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _resultadoIndicadorService.BuscarSgiAsync(idResultadoIndicador, cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }
    }
}
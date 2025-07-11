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
    public class AgenteController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AgenteController));
        private readonly IAgenteService _agenteService;
        public AgenteController(JwtService jwtService, IAgenteService agenteService) : base(jwtService)
        {
            _agenteService = agenteService;
        }


        [HttpGet]
        public async Task<ActionResult<PortalMQDIResponse>> RetornarAgentesAsync([FromQuery] AgenteFiltroViewModel filtro, CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _agenteService.BuscarAgenteAsync(filtro, cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }


        [HttpGet("buscar-tipo-relatorio")]
        public async Task<ActionResult<PortalMQDIResponse>> BuscarTipoRelatorioAsync(CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _agenteService.BuscarTipoRelatorioAsync(cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }


        [HttpGet("buscar-agente-relatorio")]
        public async Task<ActionResult<PortalMQDIResponse>> BuscarAgenteRelatorioAsync([FromQuery] AgenteFiltroViewModel filtro, CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _agenteService.BuscarAgenteRelatorioAsync(filtro, cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }


        [HttpGet("buscar-agente-download")]
        public async Task<ActionResult<PortalMQDIResponse>> BuscarAgenteDownloadRelatorioAsync([FromQuery] DownloadRelatorioFiltroViewModel filtro, CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _agenteService.BuscarAgenteDownloadRelatorioAsync(filtro, cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }


        [HttpGet("agente-consultar-medida")]
        public async Task<ActionResult<PortalMQDIResponse>> BuscarAgenteConsultarMedidaAsync([FromQuery] AgenteFiltroViewModel filtro, CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _agenteService.BuscarAgenteConsultarMedidaAsync(filtro, cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }


        [HttpGet("buscar-agente-indicador")]
        public async Task<ActionResult<PortalMQDIResponse>> BuscarAgenteIndicadorAsync([FromQuery] AgenteFiltroViewModel filtro, CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _agenteService.BuscarAgenteIndicadorAsync(filtro, cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }
    }
}

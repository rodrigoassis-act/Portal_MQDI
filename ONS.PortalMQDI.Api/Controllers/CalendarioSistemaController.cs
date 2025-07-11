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
    public class CalendarioSistemaController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CalendarioSistemaController));
        private readonly ICalendarioService _calendarioService;
        public CalendarioSistemaController(JwtService jwtService, ICalendarioService calendarioService) : base(jwtService)
        {
            _calendarioService = calendarioService;
        }


        [HttpGet("buscarDatasDisponiveisParaSelecao")]
        public async Task<ActionResult<PortalMQDIResponse>> RetornaDatasDisponiveisParaSelecaoAsync(CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _calendarioService.DatasDisponiveisParaSelecaoAsync(cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpGet("RetornaDatasDoSistema")]
        public async Task<ActionResult<PortalMQDIResponse>> RetornaDatasDoSistemaAsync([FromQuery] AgenteFiltroViewModel filtro, CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _calendarioService.RetornaDatasDoSistemaAsync(filtro, cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpGet("RecalculaCalendarioSistemaPorMudancaDeParametro")]
        public Task<bool> RecalculaCalendarioSistemaPorMudancaDeParametroAsync(CancellationToken cancellationToken)
        {
            return _calendarioService.RecalculaCalendarioSistemaPorMudancaDeParametroAsync(cancellationToken);

        }

        [HttpGet("BuscaDatasPorDataReferencia")]
        public async Task<ActionResult<PortalMQDIResponse>> BuscaDatasPorDataReferenciaAsync([FromQuery] string anoMes, CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _calendarioService.BuscaDatasPorDataReferenciaAsync(anoMes, cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }


        [HttpGet("datasDisponiveisParaDownload")]
        public async Task<ActionResult<PortalMQDIResponse>> DatasDisponiveisParaDownloadAsync(CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _calendarioService.DatasDisponiveisParaDownloadAsync(cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpGet("gerarCalendario")]
        public async Task<ActionResult<PortalMQDIResponse>> GerarCalendarioAsync([FromQuery] string anoMes, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _calendarioService.GerarCalendarioAsync(anoMes, cancellationToken)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }
    }
}

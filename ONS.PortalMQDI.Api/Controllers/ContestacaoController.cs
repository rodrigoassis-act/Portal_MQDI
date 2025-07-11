using log4net;
using Microsoft.AspNetCore.Mvc;
using ONS.PortalMQDI.Api.Attributes;
using ONS.PortalMQDI.Models.Enum;
using ONS.PortalMQDI.Models.Response;
using ONS.PortalMQDI.Models.ViewModel;
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
    public class ContestacaoController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AgenteController));
        private readonly IContestacaoService _contestacaoService;
        private readonly IResultadoIndicadorService _resultadoIndicadorService;
        public ContestacaoController(JwtService jwtService, IContestacaoService contestacaoService, IResultadoIndicadorService resultadoIndicadorService) : base(jwtService)
        {
            _contestacaoService = contestacaoService;
            _resultadoIndicadorService = resultadoIndicadorService;
        }

        [HttpPost]
        public async Task<ActionResult<PortalMQDIResponse>> CriarContestacaoAsync([FromBody] ContestacaoAnalistaViewModel contestacaoViewModel, CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _contestacaoService.CriarContestacaoAsync(contestacaoViewModel, cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpGet("AutorizacaoContestacao")]
        public async Task<ActionResult<PortalMQDIResponse>> AutorizacaoContestacaoAsync([FromQuery] string mesAno, CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _contestacaoService.AutorizacaoContestacaoAsync(mesAno, cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpGet("AutorizacaoContestacaoOns")]
        [POPAuthorize(PermissionEnum.Administrator)]
        public async Task<ActionResult<PortalMQDIResponse>> AutorizacaoResponderContestacaoAsync([FromQuery] string mesAno, CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _contestacaoService.AutorizacaoResponderContestacaoAsync(mesAno, cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpGet("data-disponivel")]
        [POPAuthorize(PermissionEnum.Administrator)]
        public async Task<ActionResult<PortalMQDIResponse>> DataDisponivelAsync(CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _contestacaoService.DataDisponivelAsync(cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpGet("buscar-agende")]
        [POPAuthorize(PermissionEnum.Administrator)]
        public async Task<ActionResult<PortalMQDIResponse>> BuscarAgenteAsync([FromQuery] string mesAno, CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _contestacaoService.BuscarAgenteAsync(mesAno, cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpGet]
        [POPAuthorize(PermissionEnum.Administrator)]
        public async Task<ActionResult<PortalMQDIResponse>> FiltroAsync([FromQuery] AcompanhamentoIndicadoresFiltroViewModel filtro, CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _contestacaoService.FiltroAsync(filtro, cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }
    }
}

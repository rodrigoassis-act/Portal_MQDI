using System;
using log4net;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ONS.PortalMQDI.Models.Response;
using ONS.PortalMQDI.Shared.Extensions;
using ONS.PortalMQDI.Services.Services;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Models.ViewModel.Filtros;

namespace ONS.PortalMQDI.Api.Controllers
{
    public class InstalacaoController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CalendarioSistemaController));
        private readonly IInstalacaoService _instalacaoService;

        public InstalacaoController(JwtService jwtService, IInstalacaoService instalacaoService) : base(jwtService)
        {
            _instalacaoService = instalacaoService;
        }


        [HttpGet("consultar-recursos")]
        public async Task<ActionResult<PortalMQDIResponse>> ConsultarRecursosAsync([FromQuery] ConsultarRecursosFiltroViewModel filtro, CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _instalacaoService.ConsultarRecursosAsync(filtro, cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpGet("medida-supervisionada-recurso")]
        public async Task<ActionResult<PortalMQDIResponse>> MedidaSupervisionadaRecursoAsync([FromQuery] string anoMes, [FromQuery] string ageMrid, CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _instalacaoService.MedidaSupervisionadaRecursoAsync(anoMes, ageMrid, cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }
    }
}

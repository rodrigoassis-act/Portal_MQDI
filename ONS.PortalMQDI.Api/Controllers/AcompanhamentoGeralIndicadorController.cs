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
    public class AcompanhamentoGeralIndicadorController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AcompanhamentoGeralIndicadorController));
        private readonly IAcompanhamentoGeralIndicadorService _acompanhamentoGeralIndicadorService;
        public AcompanhamentoGeralIndicadorController(JwtService jwtService,
            IAcompanhamentoGeralIndicadorService acompanhamentoGeralIndicadorService) : base(jwtService)
        {
            _acompanhamentoGeralIndicadorService = acompanhamentoGeralIndicadorService;
        }

        [HttpGet]
        public async Task<ActionResult<PortalMQDIResponse>> FiltroAsync([FromQuery] AcompanhamentoIndicadoresFiltroViewModel filtro, CancellationToken cancellationToke)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _acompanhamentoGeralIndicadorService.FiltroAsync(filtro, cancellationToke)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }
    }
}
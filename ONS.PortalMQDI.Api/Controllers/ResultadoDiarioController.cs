using log4net;
using Microsoft.AspNetCore.Mvc;
using ONS.PortalMQDI.Models.Response;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Services.Services;
using ONS.PortalMQDI.Shared.Extensions;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Api.Controllers
{
    public class ResultadoDiarioController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AgenteController));
        private readonly IResultadoDiarioService _resultadoDiarioService;

        public ResultadoDiarioController(JwtService jwtService, IResultadoDiarioService resultadoDiarioService) : base(jwtService)
        {
            _resultadoDiarioService = resultadoDiarioService;
        }

        [HttpGet("BuscarResultadoDiario")]
        public async Task<ActionResult<PortalMQDIResponse>> BuscarResultadoDiarioAsync(string data, string agente, string tpIndicador, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _resultadoDiarioService.BuscarResultadoDiarioAsync(data, agente, tpIndicador, cancellationToken)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }
    }
}

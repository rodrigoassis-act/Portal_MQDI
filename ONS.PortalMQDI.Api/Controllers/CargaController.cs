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
    [Route("api/[controller]")]
    [ApiController]
    public class CargaController : ControllerBase
    {
        #region CargaController
        private static readonly ILog log = LogManager.GetLogger(typeof(CargaController));
        private readonly ICargaService _cargaService;
        private readonly IMigrationService _migrationService;
        public CargaController(ICargaService cargaService, IMigrationService migrationService)
        {
            _cargaService = cargaService;
            _migrationService = migrationService;
        }

        #endregion

        [HttpGet("gerar-calendario")]
        public ActionResult<PortalMQDIResponse> GerarCalendario([FromQuery] string anoMes)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, _cargaService.GerarCalendario(anoMes)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpPost("gerar-relatorio")]
        public ActionResult<PortalMQDIResponse> GerarRelatorio([FromBody] ProcessamentoCargaFilterViewModel viewModel)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, _cargaService.GerarRelatorio(viewModel)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpPost("deletar-relatorio")]
        public async Task<ActionResult<PortalMQDIResponse>> DeletarRelatorioAsync([FromBody] ProcessamentoCargaFilterViewModel viewModel, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _cargaService.DeletarRelatorioAsync(viewModel, cancellationToken)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }


        [HttpGet("status-relatorio")]
        public ActionResult<PortalMQDIResponse> StatusRelatorio()
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, _cargaService.StatusRelatorio()));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpGet("relatorio-disponivel")]
        public async Task<ActionResult<PortalMQDIResponse>> RelatorioDisponivelAsync(CancellationToken cancellationToken)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, await _cargaService.RelatorioDisponivelAsync(cancellationToken)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }



        [HttpGet("cron-relatorio")]
        public ActionResult<PortalMQDIResponse> CronGeracaoCalendario()
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, _cargaService.GerarRelatorio()));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpGet("migrations")]
        public ActionResult<PortalMQDIResponse> MicrationRelatorio()
        {
            try
            {
                _migrationService.Processar();
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, DateTime.Now.ToString("dd/MM/yyyy")));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }
    }
}

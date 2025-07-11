using log4net;
using Microsoft.AspNetCore.Mvc;
using ONS.PortalMQDI.Models.Response;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Services.Services;
using ONS.PortalMQDI.Shared.Extensions;
using System;
using System.Net;

namespace ONS.PortalMQDI.Api.Controllers
{
    public class ExportarController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AgenteController));
        private readonly IExportarService _exportarService;
        public ExportarController(JwtService jwtService, IExportarService exportarService) : base(jwtService)
        {
            _exportarService = exportarService;
        }


        [HttpPost("exportar-consulta-indicador")]
        public IActionResult ExportarConsultaindicadores([FromBody] ExportarConsultaIndicadorViewModel filtro)
        {
            try
            {
                FileContentResult fileContentResult = _exportarService.ExportarConsultaindicadores(filtro);
                if (fileContentResult != null)
                {
                    return fileContentResult;
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, "Não foi possível gerar o arquivo."));
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpPost("exportar-relatorio-analitico")]
        public ActionResult<PortalMQDIResponse> ExportaRelatorioAnalitico([FromBody] ExportarConsultaIndicadorViewModel filtro)
        {
            try
            {
                return Ok(new PortalMQDIResponse(HttpStatusCode.OK, _exportarService.ExportaRelatorioAnalitico(filtro)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }

        [HttpPost("exportar-medida-nao-supervisionada")]
        public IActionResult ExportaMedidaNaoSupervisionada([FromBody] ExportarConsultaIndicadorViewModel filtro)
        {
            try
            {
                FileContentResult fileContentResult = _exportarService.ExportaMedidaNaoSupervisionada(filtro);
                if (fileContentResult != null)
                {
                    return fileContentResult;
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, "Não foi possível gerar o arquivo."));
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }
    }
}

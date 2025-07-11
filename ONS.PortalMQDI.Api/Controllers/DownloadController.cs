using log4net;
using Microsoft.AspNetCore.Mvc;
using ONS.PortalMQDI.Models.Enum;
using ONS.PortalMQDI.Models.Response;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Shared.Extensions;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
namespace ONS.PortalMQDI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DownloadController));
        private readonly IAwsService _awsService;
        private readonly ILogEventoService _logEventoService;
        public DownloadController(IAwsService awsService, ILogEventoService logEventoService)
        {
            _awsService = awsService;
            _logEventoService = logEventoService;
        }

        [HttpGet]
        public async Task<IActionResult> DownloadAsync([FromQuery] string arquivo, CancellationToken cancellationToken)
        {
            try
            {
                byte[] fileData = await _awsService.DownloadAsync($"Arquivos_Temporarios/{arquivo}", cancellationToken);

                if (fileData == null)
                {
                    return NotFound("Arquivo não encontrado.");
                }

                return File(fileData, "application/octet-stream", arquivo);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }


        [HttpGet("downloadRelatorio")]
        public async Task<IActionResult> DownloadRelatorioAsync([FromQuery] DownloadRelatorioFiltroViewModel filtro, CancellationToken cancellationToken)
        {
            try
            {
                var arquivo = string.Empty;
                string pasta = string.Empty;

                if (filtro.Relatorio == nameof(TipoRelatorioEnum.RAiDQ))
                {
                    pasta = filtro.IdOnsAgente;
                    arquivo = $"{filtro.Relatorio}_{filtro.IdOnsAgente}_{filtro.MesAnoSelecionado.Replace("-", "_")}.xlsx";
                }
                else if (filtro.Relatorio == nameof(TipoRelatorioEnum.RAmD))
                {
                    pasta = nameof(TipoRelatorioEnum.RAmD);
                    arquivo = $"{nameof(TipoRelatorioEnum.RAmD)}_{filtro.MesAnoSelecionado.Replace("-", "_")}.xlsx";
                }
                else
                {
                    pasta = filtro.IdOnsAgente;
                    arquivo = $"{filtro.Relatorio}_{filtro.IdOnsAgente}_{filtro.Indicador}_{filtro.MesAnoSelecionado.Replace("-", "_")}.pdf";
                }

                byte[] fileData = await _awsService.DownloadAsync($"{pasta}/{arquivo}", cancellationToken);

                if (fileData == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, new PortalMQDIResponse(HttpStatusCode.NotFound, null, $"PortalMQDI: Arquivo não encontrado."));
                }

                await _logEventoService.RegistrarEventoAsync(filtro.AgenteSelecionado, filtro.MesAnoSelecionado, PageEnum.RelatoriosMensais, cancellationToken);

                return File(fileData, "application/octet-stream", arquivo);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new PortalMQDIResponse(HttpStatusCode.InternalServerError, null, $"PortalMQDI: {ex.Message} - {ex.LogErrorWithNumber(log)}"));
            }
        }
    }
}

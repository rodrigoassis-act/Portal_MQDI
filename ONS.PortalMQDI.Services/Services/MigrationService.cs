using log4net;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Models.Enum;
using ONS.PortalMQDI.Services.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Services
{
    public class MigrationService : IMigrationService
    {
        #region MigrationService
        private readonly IAwsService _awsService;
        private readonly SharepointService _sharepointService;
        private readonly IRelatorioRepository _relatorioRepository;
        private static readonly ILog log = LogManager.GetLogger(typeof(MigrationService));


        public MigrationService(IAwsService awsService, SharepointService sharepointService, IRelatorioRepository relatorioRepository)
        {
            _awsService = awsService;
            _sharepointService = sharepointService;
            _relatorioRepository = relatorioRepository;
        }
        #endregion


        public void Processar()
        {
            var processarRelatorio = _relatorioRepository.BuscarTodosMigrationAsync();


            Task.Run(async () =>
            {
                log.Info($"Iniciou migrations total {processarRelatorio.Count()}-  {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");

                foreach (var relatorio in processarRelatorio)
                {
                    try
                    {

                        var arquivo = string.Empty;
                        string pasta = string.Empty;

                        if (relatorio.TpRelatorio.Codigo == nameof(TipoRelatorioEnum.RAiDQ))
                        {
                            pasta = relatorio.Agente.IdOns.TrimEnd();
                            arquivo = $"{relatorio.TpRelatorio.Codigo}_{relatorio.Agente.IdOns.TrimEnd()}_{relatorio.AnomesReferencia.Replace("-", "_")}.xlsx";
                        }
                        else if (relatorio.TpRelatorio.Codigo == nameof(TipoRelatorioEnum.RAmD))
                        {
                            pasta = nameof(TipoRelatorioEnum.RAmD);
                            arquivo = $"{nameof(TipoRelatorioEnum.RAmD)}_{relatorio.AnomesReferencia.Replace("-", "_")}.xlsx";
                        }
                        else
                        {
                            pasta = relatorio.Agente.IdOns.TrimEnd();
                            arquivo = $"{relatorio.TpRelatorio.Codigo}_{relatorio.Agente.IdOns.TrimEnd()}_{relatorio.TpIndicador.CodIndicador}_{relatorio.AnomesReferencia.Replace("-", "_")}.pdf";
                        }

                        byte[] fileData = _sharepointService.DownloadFile(pasta, arquivo);

                        if (fileData != null)
                        {
                            _awsService.UploadFileAsync(pasta, fileData, arquivo, CancellationToken.None).GetAwaiter().GetResult();
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Error Migrations:{ex.Message} {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                    }
                    finally
                    {
                        log.Info($"Fim migrations total {processarRelatorio.Count()}-  {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                    }
                }
            });
        }
    }
}

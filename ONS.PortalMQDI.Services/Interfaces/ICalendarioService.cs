using ONS.PortalMQDI.Models.ViewModel;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Interfaces
{
    public interface ICalendarioService
    {
        Task<List<string>> DatasDisponiveisParaSelecaoAsync(CancellationToken cancellationToke);
        Task<CalendarioViewModel> RetornaDatasDoSistemaAsync(AgenteFiltroViewModel filtro, CancellationToken cancellationToke);
        Task<bool> RecalculaCalendarioSistemaPorMudancaDeParametroAsync(CancellationToken cancellationToken);
        bool GeraCalendarioDoSistema(string maiorDataReferenciaIndicadorExistente, CancellationToken cancellationToken);
        Task<CalendarioFullViewModel> BuscaDatasPorDataReferenciaAsync(string anoMes, CancellationToken cancellationToke);
        Task<IEnumerable<string>> DatasDisponiveisParaDownloadAsync(CancellationToken cancellationToke);
        Task<bool> GerarCalendarioAsync(string anoMes, CancellationToken cancellationToken);
    }
}

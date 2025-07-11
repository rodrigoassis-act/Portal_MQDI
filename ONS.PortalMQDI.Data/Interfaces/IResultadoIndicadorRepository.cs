using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Data.Repository;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Data.Interfaces
{
    public interface IResultadoIndicadorRepository : IRepositoryAsync<ResultadoIndicador>
    {
        Task<IEnumerable<ResultadoIndicadorView>> BuscarViewPorFiltroAsync(List<DateTime> rangeDatas, string ageMrid, string tipoIndicador, bool? fragViolao, CancellationToken cancellationToke);
        Task<IEnumerable<ConsultaIndicadorSSCLView>> BuscarViewPorFiltroAsync(string datasString, string ageMrid, string tipoIndicador, bool? fragViolao, bool? isContestacao, CancellationToken cancellationToke);
        IEnumerable<ConsultaIndicadorSSCLView> BuscarViewPorFiltro(string datasString, List<string> ageMrid, string tipoIndicador, bool? fragViolao, bool? isContestacao);
        string BuscarDataReferenciaMaisRecente();
        Task<IEnumerable<SupervisaoTempoRealView>> SupervisaoTempoResultadoIndicador(string codAgente, string data, CancellationToken cancellationToken);
        Task<IEnumerable<ScadaView>> FiltrarAgentesScadaAsync(string anoMes, string CodAgente, CancellationToken cancellationToken);
        Task<IEnumerable<SgiView>> BuscarSgiViewAsync(int idResultadoIndicador, CancellationToken cancellationToken);
    }
}

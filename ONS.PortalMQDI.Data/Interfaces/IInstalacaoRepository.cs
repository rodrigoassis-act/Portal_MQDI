using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Repository;
using ONS.PortalMQDI.Data.Entity.View;

namespace ONS.PortalMQDI.Data.Interfaces
{
    public interface IInstalacaoRepository : IRepositoryAsync<Instalacao>
    {
        Task<IEnumerable<InstalacaoView>> BuscarViewPorFiltroAsync(List<DateTime> rangeDatas, string ageMrid, string tipoIndicador, bool? fragViolao, CancellationToken cancellationToke);
        Task<IEnumerable<ConsultaIndicadorInstalacaoView>> BuscarViewPorFiltroAsync(string datasString, string ageMrid, string tipoIndicador, bool? fragViolao, bool? isContestacao, CancellationToken cancellationToke);
        Task<IEnumerable<RecursoView>> ConsultarRecursosAsync(string anoMes, string ageMrid, string idInstalacao, string idCos, int idIndicador, CancellationToken cancellationToke, bool? IsContestacao = null);
        Task<IEnumerable<ConsultaIndicadorInstalacaoRecusoView>> BuscarInstalacaoRecusoroAsync(string datasString, string ageMrid, string tipoIndicador, bool? fragViolao, CancellationToken cancellationToken);
        IEnumerable<ExportarInstalacaoView> ExportarInstalacao(List<string> agentes, string anoMes, bool? isContestacao = false);
        Task<IEnumerable<ContestacaoInstalacaoRecursoView>> BuscarInstalacaoRecusoroCostentacaoViewAsync(string anoMes, List<string> idAgente, string idIndicador, bool? violacao, CancellationToken cancellationToke);
    }
}

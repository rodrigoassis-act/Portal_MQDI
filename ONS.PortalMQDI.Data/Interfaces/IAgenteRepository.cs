using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Data.Repository;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Data.Interfaces
{
    public interface IAgenteRepository : IRepositoryAsync<Agente>
    {
        Task<IEnumerable<Agente>> TodosAgentePorGrandezaAsync(string anoMesReferencia, CancellationToken cancellationToke, List<string> agentes = null);
        Task<IEnumerable<AgenteIndicadorView>> BuscarViewPorFiltroAsync(List<DateTime> rangeDatas, string ageMrid, string tipoIndicador, bool? fragViolao, CancellationToken cancellationToke);
        Task<IEnumerable<ConsultaIndicadorAgenteView>> BuscarViewPorFiltroAsync(string data, string ageMrid, string tipoIndicador, bool? fragViolao, bool? isContestacao, CancellationToken cancellationToke);
        IEnumerable<ConsultaIndicadorAgenteView> BuscarViewPorFiltro(string data, List<string> ageMrid, string tipoIndicador, bool? fragViolao, bool? isContestacao);
        Task<IEnumerable<Agente>> BuscarAgenteConsultarMedidaAsync(string anoMes, List<string> agentes, CancellationToken cancellationToke);
        Task<IEnumerable<Agente>> BuscarAgenteIndicadorAsync(string anoMes, List<string> agente, CancellationToken cancellationToke);
        Task<Agente> BuscarPorAgeMrid(string ageMrid);
    }
}

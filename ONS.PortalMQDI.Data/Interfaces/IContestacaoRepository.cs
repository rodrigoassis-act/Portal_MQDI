using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Data.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Data.Interfaces
{
    public interface IContestacaoRepository : IRepositoryAsync<Contestacao>
    {
        public Task<IEnumerable<string>> DataDisponivelAsync(CancellationToken cancellationToke);
        public Task<IEnumerable<ContestacaoAgenteView>> BuscarAgenteAsync(string anoMes, CancellationToken cancellationToke);
    }
}

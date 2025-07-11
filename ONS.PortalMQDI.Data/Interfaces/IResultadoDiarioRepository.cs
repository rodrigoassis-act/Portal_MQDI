using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Data.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Data.Interfaces
{
    public interface IResultadoDiarioRepository : IRepositoryAsync<ResultadoDiario>
    {
        Task<IEnumerable<ResultadoDiarioDCDView>> BuscarResultadoDiarioDCDAsync(string data, string ageMrid, CancellationToken cancellationToken);
        Task<IEnumerable<ResultadoDiarioDRSCView>> BuscarResultadoDiarioDRSCAsync(string data, string ageMrid, CancellationToken cancellationToken);
    }
}

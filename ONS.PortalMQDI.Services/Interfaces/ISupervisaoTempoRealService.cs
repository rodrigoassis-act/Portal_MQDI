using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Models.ViewModel;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Interfaces
{
    public interface ISupervisaoTempoRealService
    {
        public Task<IEnumerable<SupervisaoTempoRealViewModel>> BuscarAsync(SupervisaoTempoRealFiltroViewModel request, CancellationToken cancellationToken);
        public Task<IEnumerable<ScadaView>> ListaScadaAsync(SupervisaoTempoRealFiltroViewModel request, CancellationToken cancellationToken);
        public string BuscarDatas();
    }
}

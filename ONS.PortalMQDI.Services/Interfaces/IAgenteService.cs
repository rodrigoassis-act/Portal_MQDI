using ONS.PortalMQDI.Models.ViewModel;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Interfaces
{
    public interface IAgenteService
    {
        Task<IEnumerable<SelectItemViewModel<string>>> BuscarAgenteAsync(AgenteFiltroViewModel filtro, CancellationToken cancellationToke);
        Task<IEnumerable<SelectItemViewModel<string>>> BuscarAgenteConsultarMedidaAsync(AgenteFiltroViewModel filtro, CancellationToken cancellationToke);
        Task<IEnumerable<SelectItemRelatorioViewModel>> BuscarAgenteDownloadRelatorioAsync(DownloadRelatorioFiltroViewModel filtro, CancellationToken cancellationToke);
        Task<IEnumerable<SelectItemViewModel<string>>> BuscarAgenteIndicadorAsync(AgenteFiltroViewModel filtro, CancellationToken cancellationToke);
        Task<IEnumerable<SelectItemRelatorioViewModel>> BuscarAgenteRelatorioAsync(AgenteFiltroViewModel filtro, CancellationToken cancellationToke);
        Task<IEnumerable<SelectItemViewModel<int>>> BuscarTipoRelatorioAsync(CancellationToken cancellationToke);
    }
}

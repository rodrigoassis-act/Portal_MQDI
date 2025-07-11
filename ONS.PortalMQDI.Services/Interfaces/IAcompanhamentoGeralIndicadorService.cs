using ONS.PortalMQDI.Models.ViewModel;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Interfaces
{
    public interface IAcompanhamentoGeralIndicadorService
    {
        Task<AcompanhamentoIndicadoresViewModel> FiltroAsync(AcompanhamentoIndicadoresFiltroViewModel filtro, CancellationToken cancellationToke);
    }
}

using ONS.PortalMQDI.Models.ViewModel;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Interfaces
{
    public interface IResultadoIndicadorService
    {
        Task<List<SgiViewModel>> BuscarSgiAsync(int idResultadoIndicador, CancellationToken cancellationToke);
        Task<ResultadoIndicadorViewModel> FiltroAsync(AcompanhamentoIndicadoresFiltroViewModel filtro, CancellationToken cancellationToke, bool isContestacao = false);
        Task<List<SelectItemViewModel<string>>> ListaTpIndicadorAsync(CancellationToken cancellationToke);
    }
}

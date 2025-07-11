using ONS.PortalMQDI.Models.ViewModel;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Interfaces
{
    public interface IContestacaoService
    {
        Task<bool> AutorizacaoContestacaoAsync(string mesAno, CancellationToken cancellationToke);
        Task<bool> AutorizacaoResponderContestacaoAsync(string mesAno, CancellationToken cancellationToke);
        Task<List<SelectItemViewModel<string>>> BuscarAgenteAsync(string mesAno, CancellationToken cancellationToke);
        Task<ContestacaoAnalistaViewModel> CriarContestacaoAsync(ContestacaoAnalistaViewModel contestacaoViewModel, CancellationToken cancellationToke);
        Task<List<string>> DataDisponivelAsync(CancellationToken cancellationToke);
        Task<ResultadoIndicadorViewModel> FiltroAsync(AcompanhamentoIndicadoresFiltroViewModel filtro, CancellationToken cancellationToke);
    }
}

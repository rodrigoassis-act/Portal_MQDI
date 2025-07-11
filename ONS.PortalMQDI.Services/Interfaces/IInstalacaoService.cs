using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Models.ViewModel;
using ONS.PortalMQDI.Models.ViewModel.Filtros;

namespace ONS.PortalMQDI.Services.Interfaces
{
    public interface IInstalacaoService
    {
        Task<List<ConsultaRecursoItemViewModel>> ConsultarRecursosAsync(ConsultarRecursosFiltroViewModel filtro, CancellationToken cancellationToke);
        Task<List<MedidaSupervisionadaRecursoViewModel>> MedidaSupervisionadaRecursoAsync(string anoMes, string ageMrid, CancellationToken cancellationToke);
        List<MedidaSupervisionadaRecursoViewModel> RetornaMedidasSupervisionadasRecursos(IEnumerable<InstalacaoConsultarMedidaView> queryInstalacao);
    }
}

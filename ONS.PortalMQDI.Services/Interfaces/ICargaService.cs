using ONS.PortalMQDI.Models.Model;
using ONS.PortalMQDI.Models.ViewModel;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Interfaces
{
    public interface ICargaService
    {
        Task<RelatorioDisponivelViewModel> RelatorioDisponivelAsync(CancellationToken cancellationToken);
        Task<string> DeletarRelatorioAsync(ProcessamentoCargaFilterViewModel viewModel, CancellationToken cancellationToken);
        bool GerarCalendario(string anoMes);
        bool GerarRelatorio(ProcessamentoCargaFilterViewModel viewModel);
        StatusCarga StatusRelatorio();
        bool GerarRelatorio();
    }
}

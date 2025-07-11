using ONS.PortalMQDI.Models.ViewModel;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Interfaces
{
    public interface IResultadoDiarioService
    {
        Task<List<ResultadoDiarioViewModel>> BuscarResultadoDiarioAsync(string data, string agente, string tpIndicador, CancellationToken cancellationToken);
    }
}

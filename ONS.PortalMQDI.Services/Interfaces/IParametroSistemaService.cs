using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Models.ViewModel;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Interfaces
{
    public interface IParametroSistemaService
    {
        Task<List<ParametroSistemaViewModel>> BuscaValorLimiteDeIndicadoresAsync(CancellationToken cancellationToke);
        Task<List<ParametroSistemaViewModel>> BuscaValorParametroSistemaAsync(CancellationToken cancellationToke);
        Task<ValorParametroSistema> AdicionarNovoParametroAsync(ParametroSistemaViewModel param, CancellationToken cancellationToke);
        Task<bool> ExisteRegistroFaltanteNoCalendarioAsync(string anoMes, CancellationToken cancellationToken);
        Task<bool> AdicionaRegistroFaltanteNoCalendarioAsync(string anoMes, CancellationToken cancellationToken);
        Task<int> LimpezaCompletaAsync(string anoMes, CancellationToken cancellationToken);
    }
}
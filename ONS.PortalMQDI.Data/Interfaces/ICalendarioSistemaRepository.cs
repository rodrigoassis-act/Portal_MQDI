using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Data.Interfaces
{
    public interface ICalendarioSistemaRepository : IRepositoryAsync<CalendarioSistema>
    {
        Task<CalendarioSistema> BuscaDatasPorDataReferenciaAsync(string data, CancellationToken cancellationToken);
        IEnumerable<CalendarioSistema> PegarTodosCalendario();
        bool AdicionarRegistro(CalendarioSistema calenadrio);
        Task<IEnumerable<string>> VerificarLocalidadeExistente(string anoMes, CancellationToken cancellationToken);
    }
}

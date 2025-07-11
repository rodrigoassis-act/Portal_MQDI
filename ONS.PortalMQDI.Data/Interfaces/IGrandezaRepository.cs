using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Data.Repository;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Data.Interfaces
{
    public interface IGrandezaRepository : IRepositoryAsync<Grandeza>
    {
        IEnumerable<DadosInstalacaoView> BuscarInstalacaoRangerData(List<DateTime> rangeDatas);
        Task<IEnumerable<InstalacaoConsultarMedidaView>> InstalacaoConsultarMedidaAsync(string ageMrid, string anoMes, CancellationToken cancellationToke);
        IEnumerable<InstalacaoConsultarMedidaView> InstalacaoConsultarMedida(List<string> ageMrid, string anoMes);
    }
}

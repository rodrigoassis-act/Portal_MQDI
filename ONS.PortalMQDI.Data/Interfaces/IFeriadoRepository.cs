using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Repository;
using System.Collections.Generic;

namespace ONS.PortalMQDI.Data.Interfaces
{
    public interface IFeriadoRepository : IRepositoryAsync<Feriado>
    {
        IEnumerable<Feriado> ListaFeriadosNoMes(string anoMesReferencia);
    }
}

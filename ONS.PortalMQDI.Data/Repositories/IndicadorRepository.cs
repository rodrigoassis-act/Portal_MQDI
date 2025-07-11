using goobeeteams.Entity.Repositories;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;

namespace ONS.PortalMQDI.Data.Repositories
{
    public class IndicadorRepository : RepositoryAsync<Indicador>, IIndicadorRepository
    {
        public IndicadorRepository(PortalMQDIDbContext context) : base(context)
        { }

    }
}
using goobeeteams.Entity.Repositories;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;

namespace ONS.PortalMQDI.Data.Repositories
{
    public class TpIndicadorRepository : RepositoryAsync<TpIndicador>, ITpIndicadorRepository
    {
        public TpIndicadorRepository(PortalMQDIDbContext context) : base(context)
        { }

    }
}
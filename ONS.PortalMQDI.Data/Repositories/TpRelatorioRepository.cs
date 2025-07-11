using goobeeteams.Entity.Repositories;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;

namespace ONS.PortalMQDI.Data.Repositories
{
    public class TpRelatorioRepository : RepositoryAsync<TpRelatorio>, ITpRelatorioRepository
    {
        public TpRelatorioRepository(PortalMQDIDbContext context) : base(context)
        { }

    }
}
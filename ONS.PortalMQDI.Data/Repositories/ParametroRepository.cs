using goobeeteams.Entity.Repositories;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;

namespace ONS.PortalMQDI.Data.Repositories
{
    public class ParametroRepository : RepositoryAsync<Parametro>, IParametroRepository
    {
        public ParametroRepository(PortalMQDIDbContext context) : base(context)
        { }

    }
}
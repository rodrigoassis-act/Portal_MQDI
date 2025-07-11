using goobeeteams.Entity.Repositories;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;

namespace ONS.PortalMQDI.Data.Repositories
{
    public class CosRepository : RepositoryAsync<Cos>, ICosRepository
    {
        public CosRepository(PortalMQDIDbContext context) : base(context)
        { }

    }
}
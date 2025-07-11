using goobeeteams.Entity.Repositories;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;

namespace ONS.PortalMQDI.Data.Repositories
{
    public class RequisitoRepository : RepositoryAsync<Requisito>, IRequisitoRepository
    {
        public RequisitoRepository(PortalMQDIDbContext context) : base(context)
        { }

    }
}
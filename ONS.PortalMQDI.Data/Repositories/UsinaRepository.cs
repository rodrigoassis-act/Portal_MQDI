using goobeeteams.Entity.Repositories;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;

namespace ONS.PortalMQDI.Data.Repositories
{
    public class UsinaRepository : RepositoryAsync<Usina>, IUsinaRepository
    {
        public UsinaRepository(PortalMQDIDbContext context) : base(context)
        { }

    }
}
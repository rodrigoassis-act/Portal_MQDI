using goobeeteams.Entity.Repositories;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;

namespace ONS.PortalMQDI.Data.Repositories
{
    public class AnalistaCosRepository : RepositoryAsync<AnalistaCos>, IAnalistaCosRepository
    {
        public AnalistaCosRepository(PortalMQDIDbContext context) : base(context)
        { }
    }
}
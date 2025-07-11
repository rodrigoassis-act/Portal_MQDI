using goobeeteams.Entity.Repositories;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;

namespace ONS.PortalMQDI.Data.Repositories

{
    public class UtrCdRepository : RepositoryAsync<UtrCd>, IUtrCdRepository
    {
        public UtrCdRepository(PortalMQDIDbContext context) : base(context)
        { }

    }
}
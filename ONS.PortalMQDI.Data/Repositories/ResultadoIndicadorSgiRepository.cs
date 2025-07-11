using goobeeteams.Entity.Repositories;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;

namespace ONS.PortalMQDI.Data.Repositories
{
    public class ResultadoIndicadorSgiRepository : RepositoryAsync<ResultadoIndicadorSgi>, IResultadoIndicadorSgiRepository
    {
        public ResultadoIndicadorSgiRepository(PortalMQDIDbContext context) : base(context)
        { }

    }
}
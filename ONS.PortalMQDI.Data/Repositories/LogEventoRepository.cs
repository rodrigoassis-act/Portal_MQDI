using goobeeteams.Entity.Repositories;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;

namespace ONS.PortalMQDI.Data.Repositories
{
    public class LogEventoRepository : RepositoryAsync<LogEvento>, ILogEventoRepository
    {
        public LogEventoRepository(PortalMQDIDbContext context) : base(context)
        { }
    }
}

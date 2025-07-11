using goobeeteams.Entity.Repositories;
using Microsoft.EntityFrameworkCore;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Data.Repositories
{
    public class ResultadoDiarioRepository : RepositoryAsync<ResultadoDiario>, IResultadoDiarioRepository
    {
        private readonly PortalMQDIDbContext _context;

        public ResultadoDiarioRepository(PortalMQDIDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ResultadoDiarioDCDView>> BuscarResultadoDiarioAsync(string data, string agente, string idRecurso, string idIndicador, CancellationToken cancellationToken)
        {
            var parametros = new Dictionary<string, string>
            {
                {"data", data},
                {"agente", agente},
                {"idRecurso", idRecurso},
                {"idIndicador", idIndicador}
            };

            var sqlQuery = GetQuery("QUERY_RESULTADO_DIARIO", parametros).Replace("\r\n", " ");

            return await _context.Set<ResultadoDiarioDCDView>()
                .FromSqlRaw(sqlQuery, parametros.Values.ToArray())
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);
        }

        public async Task<IEnumerable<ResultadoDiarioDCDView>> BuscarResultadoDiarioDCDAsync(string data, string ageMrid, CancellationToken cancellationToken)
        {
            var parametros = new Dictionary<string, string>
            {
                {"data", data},
                {"agente", ageMrid},
            };

            var sqlQuery = GetQuery("QUERY_RESULTADO_DIARIO_DCD", parametros).Replace("\r\n", " ");

            return await _context.Set<ResultadoDiarioDCDView>()
                .FromSqlRaw(sqlQuery, parametros.Values.ToArray())
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);
        }

        public async Task<IEnumerable<ResultadoDiarioDRSCView>> BuscarResultadoDiarioDRSCAsync(string data, string ageMrid, CancellationToken cancellationToken)
        {
            var parametros = new Dictionary<string, string>
            {
                {"data", data},
                {"agente", ageMrid},
            };

            var sqlQuery = GetQuery("QUERY_RESULTADO_DIARIO_DRSC", parametros).Replace("\r\n", " ");

            return await _context.Set<ResultadoDiarioDRSCView>()
                .FromSqlRaw(sqlQuery, parametros.Values.ToArray())
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);
        }
    }
}
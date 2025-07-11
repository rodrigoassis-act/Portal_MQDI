using goobeeteams.Entity.Repositories;
using Microsoft.EntityFrameworkCore;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Data.Repositories
{
    public class ContestacaoRepository : RepositoryAsync<Contestacao>, IContestacaoRepository
    {
        public ContestacaoRepository(PortalMQDIDbContext context) : base(context)
        { }

        public async Task<IEnumerable<ContestacaoAgenteView>> BuscarAgenteAsync(string anoMes, CancellationToken cancellationToke)
        {
            var parametros = new Dictionary<string, string>
            {
                {"filtroAnoMes", !string.IsNullOrEmpty(anoMes) ? $"where r.anomes_referencia = '{anoMes.Replace("/", "-")}'" : string.Empty}
            };

            var sqlQuery = GetQuery("QUERY_BUSCAR_AGENTE", parametros);

            return await _context.Set<ContestacaoAgenteView>()
                .FromSqlRaw(sqlQuery)
                .AsNoTracking()
                .ToArrayAsync(cancellationToke);
        }


        public async Task<IEnumerable<string>> DataDisponivelAsync(CancellationToken cancellationToke)
        {
            return await _context.Contestacoes
                .AsNoTracking()
                .Include(c => c.ResultadoIndicador)
                .Select(r => r.ResultadoIndicador.AnoMesReferencia)
                .Distinct()
                .OrderByDescending(data => data)
                .ToArrayAsync(cancellationToke);
        }

    }
}
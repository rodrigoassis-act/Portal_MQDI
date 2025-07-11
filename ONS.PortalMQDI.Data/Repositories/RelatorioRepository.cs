using goobeeteams.Entity.Repositories;
using Microsoft.EntityFrameworkCore;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Data.Repositories
{
    public class RelatorioRepository : RepositoryAsync<Relatorio>, IRelatorioRepository
    {
        public RelatorioRepository(PortalMQDIDbContext context) : base(context)
        { }

        public async Task<IEnumerable<string>> BuscarDatasDownloadAsync(CancellationToken cancellationToke)
        {
            return await _context.Relatorios
                .AsNoTracking()
                .Select(c => c.AnomesReferencia.Replace("-", "/"))
                .Distinct()
                .OrderByDescending(c => c)
                .ToListAsync(cancellationToke);
        }

        public async Task<IEnumerable<Relatorio>> BuscarAgenteAsync(Expression<Func<Relatorio, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _context
                .Relatorios
                .Include(c => c.Agente)
                .Include(c => c.TpIndicador)
                .Include(c => c.TpRelatorio)
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> DeletarPorFiltroAsync(Expression<Func<Relatorio, bool>> predicate, CancellationToken cancellationToken)
        {

            var entryDelete = await _context.Relatorios.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);

            _context.Relatorios.RemoveRange(entryDelete);

            return await _context.SaveChangesAsync(cancellationToken) > 0 ? true : false;
        }

        public async Task<IEnumerable<Relatorio>> BuscarTodosRelatorioAsync(CancellationToken cancellationToken)
        {
            return await _context.Relatorios.Where(c => c.AgenteId != "ons").Include(c => c.Agente).AsNoTracking().ToListAsync(cancellationToken);
        }

        public bool ConsultarCentroProcessamento(string centro, string anoMes)
        {
            var parametros = new Dictionary<string, string>
            {
                {"centro", centro},
                {"anoMes", anoMes},
            };

            var sqlQuery = GetQuery("QUERY_GERAR_CALENDARIO", parametros);
            return _context.Database.ExecuteSqlRaw(sqlQuery) > 0;
        }

        public IEnumerable<Relatorio> BuscarTodosMigrationAsync()
        {
            return _context.Relatorios.Include(c => c.Agente).Include(c => c.TpIndicador).Include(c => c.TpRelatorio).AsNoTracking();
        }
    }
}
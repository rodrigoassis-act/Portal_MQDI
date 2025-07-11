using goobeeteams.Entity.Repositories;
using Microsoft.EntityFrameworkCore;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Data.Repositories
{
    public class ValorParametroSistemaRepository : RepositoryAsync<ValorParametroSistema>, IValorParametroSistemaRepository
    {
        public ValorParametroSistemaRepository(PortalMQDIDbContext context) : base(context)
        { }

        public async Task<IEnumerable<ValorParametroSistema>> BuscarValorLimiteAsync(CancellationToken cancellationToke)
        {
            return await _context.ValorParametroSistema
                .AsNoTracking()
                .Include(c => c.ParametroSistema)
                .Where(c => c.ParametroSistema.NomeParametro.StartsWith("Valor Limite"))
                .ToArrayAsync(cancellationToke);
        }

        public async Task<ValorParametroSistema> BuscarParametroTerminoPeriodoAnaliseContestacaoAsync(CancellationToken cancellationToke)
        {
            return await _context.ValorParametroSistema
                .AsNoTracking()
                .Include(c => c.ParametroSistema)
                .FirstOrDefaultAsync(c => c.ParametroSistema.NomeParametro == ApplicationConstants.ParametroTerminoPeriodoAnaliseContestacao, cancellationToke);
        }

        public async Task<IEnumerable<ValorParametroSistema>> BuscaValorParametroSistemaAsync(CancellationToken cancellationToken)
        {
            return await _context.ValorParametroSistema
                .AsNoTracking()
                .Include(c => c.ParametroSistema)
                .Where(c => c.DataFimVigencia == null)
                .ToArrayAsync(cancellationToken);
        }

        public async Task<ValorParametroSistema> RetornaValorDeParametroAtualPorIdParametroAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.ValorParametroSistema
                .AsNoTracking()
                .Include(c => c.ParametroSistema)
                .Where(c => c.IdParametro == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public ValorParametroSistema RetornaValorAtualDoParametroPorNome(string nomeParametro)
        {
            return _context.ValorParametroSistema.FirstOrDefault(param => param.ParametroSistema.NomeParametro == nomeParametro && param.DataFimVigencia == null);
        }

        public Task<int> ExecutarQueryLimpezaCompletaAsync(string anoMes, CancellationToken cancellationToken)
        {
            var query = $@"
                        begin;
                        Declare @AnoMesReferencia Char(7);
                        Set @AnoMesReferencia = '{anoMes}';

                        delete from tb_sgi                          where num_ons in (select num_ons from tb_resultadoindicador_sgi where id_resultadoindicador in (select id_resultadoindicador from tb_resultadoindicador where anomes_referencia = @AnoMesReferencia));
                        delete from tb_resultadoindicador_sgi       where id_resultadoindicador in (select id_resultadoindicador from tb_resultadoindicador where anomes_referencia = @AnoMesReferencia);
                        delete from tb_resultadoindicadorexpurgo    where id_resultadoindicador in (select id_resultadoindicador from tb_resultadoindicador where anomes_referencia = @AnoMesReferencia);
                        delete from tb_resultadocontestacao			where id_contestacao in (select id_contestacao from tb_contestacao where id_resultadoindicador in (select id_resultadoindicador from tb_resultadoindicador where anomes_referencia = @AnoMesReferencia));
                        delete from tb_contestacao                  where id_resultadoindicador in (select id_resultadoindicador from tb_resultadoindicador where anomes_referencia = @AnoMesReferencia);
                        delete from tb_calendariosistema            where anomes_referencia = @AnoMesReferencia;
                        delete from tb_relatorio                    where anomes_referencia = @AnoMesReferencia;
                        delete from tb_resultadoindicador           where anomes_referencia = @AnoMesReferencia;
                        end;
                        ";

            return _context.Database.ExecuteSqlRawAsync(query, cancellationToken);
        }

        public IEnumerable<ValorParametroSistema> RetornaParametrosBy(Expression<Func<ValorParametroSistema, bool>> predicate)
        {
            return _context.ValorParametroSistema.Include(c => c.ParametroSistema).AsNoTracking().Where(predicate).ToArray();
        }
    }
}
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Context;
using Microsoft.EntityFrameworkCore;
using ONS.PortalMQDI.Data.Interfaces;
using goobeeteams.Entity.Repositories;
using ONS.PortalMQDI.Data.Entity.View;

namespace ONS.PortalMQDI.Data.Repositories
{
    public class InstalacaoRepository : RepositoryAsync<Instalacao>, IInstalacaoRepository
    {
        public InstalacaoRepository(PortalMQDIDbContext context) : base(context)
        { }

        public async Task<IEnumerable<InstalacaoView>> BuscarViewPorFiltroAsync(List<DateTime> rangeDatas, string ageMrid, string tipoIndicador, bool? fragViolao, CancellationToken cancellationToke)
        {
            var datas = rangeDatas.Select(data => data.ToString("yyyy-MM")).ToList();
            var datasString = string.Join("','", datas);

            var parametros = new Dictionary<string, string>
            {
                {"datasString", datasString},
                {"ageMrid", ageMrid},
                { "filtroTipoIndicador", string.IsNullOrEmpty(tipoIndicador) ? "" : $" and ti.cod_tpindicador = '{tipoIndicador}'" },
                { "filtroFragViolao", !fragViolao.HasValue ? "" : $" and ri.flg_violacaomensal = {(fragViolao.Value ? "1" : "0")}" }
            };

            var sqlQuery = GetQuery("QUERY_BUSCAR_INSTALACAO_VIEW_POR_FILTRO", parametros);

            return await _context.Set<InstalacaoView>()
                .FromSqlRaw(sqlQuery)
                .AsNoTracking()
                .ToArrayAsync(cancellationToke);
        }

        public async Task<IEnumerable<ConsultaIndicadorInstalacaoView>> BuscarViewPorFiltroAsync(string datasString, string ageMrid, string tipoIndicador, bool? fragViolao, bool? isContestacao, CancellationToken cancellationToke)
        {
            var parametros = new Dictionary<string, string>
            {
                {"datasString", datasString},
                {"ageMrid", ageMrid},
                { "filtroTipoIndicador", string.IsNullOrEmpty(tipoIndicador) ? "" : $" and ti.cod_tpindicador = '{tipoIndicador}'" },
                { "filtroFragViolao", !fragViolao.HasValue ? "" : $" and ri.flg_violacaomensal = {(fragViolao.Value ? "1" : "0")}" },
                { "filtroIsContestacao", !isContestacao.HasValue || !isContestacao.Value ? "" : " and c.id_contestacao IS NOT NULL" }
            };

            var sqlQuery = GetQuery("QUERY_BUSCAR_CONSULTA_INDICADOR_INSTALACAO_POR_FILTRO", parametros);

            return await _context.Set<ConsultaIndicadorInstalacaoView>()
                .FromSqlRaw(sqlQuery)
                .AsNoTracking()
                .ToArrayAsync(cancellationToke);
        }

        public async Task<IEnumerable<RecursoView>> ConsultarRecursosAsync(string anoMes, string ageMrid, string idInstalacao, string idCos, int idIndicador, CancellationToken cancellationToken, bool? IsContestacao = null)
        {
            var parametros = new Dictionary<string, string>
            {
                {"anoMes", anoMes},
                {"ageMrid", ageMrid},
                {"idInstalacao", idInstalacao},
                {"idCos", idCos},
                {"idIndicador", idIndicador.ToString()},
                {"filtroIsContestacao", IsContestacao.HasValue && IsContestacao.Value ? " and c.dsc_contestacao IS NOT NULL" : ""}
            };

            var sqlQuery = GetQuery("QUERY_CONSULTAR_RECURSOS", parametros);

            return await _context.Set<RecursoView>()
                .FromSqlRaw(sqlQuery)
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);
        }

        public async Task<IEnumerable<ConsultaIndicadorInstalacaoRecusoView>> BuscarInstalacaoRecusoroAsync(string datasString, string ageMrid, string tipoIndicador, bool? fragViolao, CancellationToken cancellationToken)
        {
            var parametros = new Dictionary<string, string>
            {
                {"datasString", datasString},
                {"ageMrid", "%" + ageMrid + "%"},
                {"tipoIndicador", tipoIndicador},
                {"fragViolaoValue", fragViolao.HasValue ? (fragViolao.Value ? "1" : "0") : string.Empty}
            };

            var sqlQuery = GetQuery("QUERY_BUSCAR_INSTALACAO_RECURSO", parametros);

            return await _context.Set<ConsultaIndicadorInstalacaoRecusoView>()
                .FromSqlRaw(sqlQuery, parametros.Values.ToArray())
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);
        }

        public IEnumerable<ExportarInstalacaoView> ExportarInstalacao(List<string> agentes, string anoMes, bool? isContestacao = false)
        {
            var agentesString = string.Join("','", agentes);
            var parametros = new Dictionary<string, string>
            {
                {"anoMes", anoMes},
                {"agentesString", agentesString}
            };

            var sqlQuery = GetQuery("QUERY_EXPORTAR_INSTALACAO", parametros);

            if (isContestacao.HasValue && isContestacao.Value)
            {
                sqlQuery += " AND ci.instalacaoAnalista IS NOT NULL AND cr.recursoAnalista IS NOT NULL";
            }

            return _context.Set<ExportarInstalacaoView>()
                .FromSqlRaw(sqlQuery, parametros.Values.ToArray())
                .AsNoTracking()
                .ToArray();
        }

        public async Task<IEnumerable<ContestacaoInstalacaoRecursoView>> BuscarInstalacaoRecusoroCostentacaoViewAsync(string anoMes, List<string> idAgente, string idIndicador, bool? violacao, CancellationToken cancellationToke)
        {
            var agentesString = string.Join("','", idAgente);
            var parametros = new Dictionary<string, string>
            {
                {"AnoMes", anoMes},
                {"agentesString", agentesString}
            };

            var sqlQuery = GetQuery("QUERY_CONTESTACAO_INSTALACAO_RECURSO", parametros);

            return await _context.Set<ContestacaoInstalacaoRecursoView>()
                .FromSqlRaw(sqlQuery, parametros.Values.ToArray())
                .AsNoTracking()
                .ToArrayAsync(cancellationToke);
        }
    }
}
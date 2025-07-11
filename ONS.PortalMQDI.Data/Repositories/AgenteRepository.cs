using Microsoft.EntityFrameworkCore;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Data.Repositories
{
    public class AgenteRepository : RepositoryAsync<Agente>, IAgenteRepository
    {
        public AgenteRepository(PortalMQDIDbContext context) : base(context)
        { }

        public async Task<IEnumerable<Agente>> BuscarAgenteConsultarMedidaAsync(string anoMes, List<string> agentes, CancellationToken cancellationToke)
        {
            var parametros = new Dictionary<string, string>
            {
                {"anoMes", anoMes},
                {"filtroAgentes", (agentes != null && agentes.Count > 0) ? " AND a.ido_ons IN (" + string.Join(",", agentes.Select(a => $"'{a}'")) + ")" : string.Empty}
            };

            var sqlQuery = GetQuery("QUERY_BUSCAR_AGENTE_CONSULTAR_MEDIDA", parametros);

            return await _context.Set<Agente>()
                .FromSqlRaw(sqlQuery)
                .AsNoTracking()
                .ToArrayAsync(cancellationToke);
        }


        public async Task<IEnumerable<Agente>> BuscarAgenteIndicadorAsync(string anoMes, List<string> agentes, CancellationToken cancellationToke)
        {
            var parametros = new Dictionary<string, string>
            {
                {"anoMes", anoMes},
                {"filtroAgentes", (agentes != null && agentes.Count > 0) ? " AND a.ido_ons IN (" + string.Join(",", agentes.Select(a => $"'{a}'")) + ")" : string.Empty}
            };

            var sqlQuery = GetQuery("QUERY_BUSCAR_AGENTE_INDICADOR", parametros);

            return await _context.Set<Agente>()
                .FromSqlRaw(sqlQuery)
                .AsNoTracking()
                .ToArrayAsync(cancellationToke);
        }

        public async Task<Agente> BuscarPorAgeMrid(string ageMrid)
        {
            return await _context.Agente.FirstOrDefaultAsync(c => c.AgeMrid == ageMrid);
        }

        public IEnumerable<ConsultaIndicadorAgenteView> BuscarViewPorFiltro(string data, List<string> ageMrid, string tipoIndicador, bool? fragViolao, bool? isContestacao)
        {
            var parametros = new Dictionary<string, string>
            {
                {"data", data},
                {"agentes", string.Join("','", ageMrid)},
                {"filtroTipoIndicador", !string.IsNullOrEmpty(tipoIndicador) ? $" and ti.cod_tpindicador = '{tipoIndicador}'" : string.Empty},
                {"filtroFragViolao", fragViolao.HasValue ? $" and ri.flg_violacaomensal = {(fragViolao.Value ? "1" : "0")}" : string.Empty},
                {"filtroContestacao", isContestacao.HasValue && isContestacao.Value ? " and c.id_contestacao IS NOT NULL" : string.Empty}
            };

            var sqlQuery = GetQuery("QUERY_CONSULTA_INDICADOR_AGENTE_VIEW_POR_FILTRO", parametros);

            return _context.Set<ConsultaIndicadorAgenteView>()
                .FromSqlRaw(sqlQuery)
                .AsNoTracking()
                .ToArray();
        }


        public async Task<IEnumerable<AgenteIndicadorView>> BuscarViewPorFiltroAsync(List<DateTime> rangeDatas, string ageMrid, string tipoIndicador, bool? fragViolao, CancellationToken cancellationToke)
        {
            var datas = rangeDatas.Select(data => data.ToString("yyyy-MM")).ToList();
            var datasString = string.Join("','", datas);

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "datas", datasString },
                { "ageMrid", ageMrid },
                { "filtroTipoIndicador", string.IsNullOrEmpty(tipoIndicador) ? "" : $" and ti.cod_tpindicador = '{tipoIndicador}'" },
                { "filtroFragViolao", !fragViolao.HasValue ? "" : $" and ri.flg_violacaomensal = {(fragViolao.Value ? "1" : "0")}" }
            };

            var sqlQuery = GetQuery("QUERY_BUSCAR_VIEW_POR_FILTRO", parameters);

            return await _context.Set<AgenteIndicadorView>()
                .FromSqlRaw(sqlQuery)
                .AsNoTracking()
                .ToArrayAsync(cancellationToke); 
        }


        public async Task<IEnumerable<ConsultaIndicadorAgenteView>> BuscarViewPorFiltroAsync(string dataString, string ageMrid, string tipoIndicador, bool? fragViolao, bool? isContestacao, CancellationToken cancellationToke)
        {
            var parametros = new Dictionary<string, string>
            {
                {"dataString", dataString},
                {"ageMrid", ageMrid},
                {"filtroTipoIndicador", !string.IsNullOrEmpty(tipoIndicador) ? $" and ti.cod_tpindicador = '{tipoIndicador}'" : string.Empty},
                {"filtroFragViolao", fragViolao.HasValue ? $" and ri.flg_violacaomensal = {(fragViolao.Value ? "1" : "0")}" : string.Empty},
                {"filtroContestacao", (isContestacao.HasValue && isContestacao.Value) ? " and c.id_contestacao IS NOT NULL" : string.Empty}
            };

            var sqlQuery = GetQuery("QUERY_BUSCAR_VIEW_POR_FILTRO_CONSULTAINDICADORAGENTE", parametros);

            return await _context.Set<ConsultaIndicadorAgenteView>()
                .FromSqlRaw(sqlQuery)
                .AsNoTracking()
                .ToArrayAsync(cancellationToke);
        }


        public async Task<IEnumerable<Agente>> TodosAgentePorGrandezaAsync(string anoMesReferencia, CancellationToken cancellationToken, List<string> agentes = null)
        {
            var parametros = new Dictionary<string, string>
            {
                {"filtroAnoMesReferencia", String.IsNullOrEmpty(anoMesReferencia) ? string.Empty : $"WHERE a.anomes_referencia = '{anoMesReferencia}' " },
                {"filtroAgentes", agentes != null && agentes.Any() ? $" AND a.ido_ons IN ({string.Join(",", agentes.Select(a => $"'{a}'"))})" : string.Empty}
            };

            var sqlQuery = GetQuery("QUERY_TODOS_AGENTE_POR_GRANDEZA", parametros);

            return await _context.Set<Agente>()
                .FromSqlRaw(sqlQuery)
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);
        }

    }
}

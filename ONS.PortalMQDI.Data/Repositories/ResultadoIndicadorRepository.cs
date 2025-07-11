using goobeeteams.Entity.Repositories;
using Microsoft.EntityFrameworkCore;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class ResultadoIndicadorRepository : RepositoryAsync<ResultadoIndicador>, IResultadoIndicadorRepository
{
    private readonly PortalMQDIDbContext _context;

    public ResultadoIndicadorRepository(PortalMQDIDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ResultadoIndicadorView>> BuscarViewPorFiltroAsync(List<DateTime> rangeDatas, string ageMrid, string tipoIndicador, bool? fragViolao, CancellationToken cancellationToken)
    {
        var datas = rangeDatas.Select(data => data.ToString("yyyy-MM")).ToList();
        var datasString = string.Join("','", datas);

        var parametros = new Dictionary<string, string>
        {
            {"datasString", datasString},
            {"ageMrid", ageMrid},
            {"filtroTipoIndicador", string.IsNullOrEmpty(tipoIndicador) ? "" : $" AND ti.cod_tpindicador = '{tipoIndicador}'"},
            {"filtroFragViolao", fragViolao.HasValue ? $" AND ur.flg_violacaomensal = {(fragViolao.Value ? "1" : "0")}" : ""}
        };

        var sqlQuery = GetQuery("QUERY_BUSCAR_VIEW_POR_FILTRO_RESULTADOINDICADOR", parametros);

        return await _context.Set<ResultadoIndicadorView>()
            .FromSqlRaw(sqlQuery)
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }

    public async Task<IEnumerable<ConsultaIndicadorSSCLView>> BuscarViewPorFiltroAsync(string datasString, string ageMrid, string tipoIndicador, bool? fragViolao, bool? isContestacao, CancellationToken cancellationToken)
    {
        var parametros = new Dictionary<string, string>
        {
            {"datasString", datasString},
            {"ageMrid", ageMrid},
            {"filtroTipoIndicador", string.IsNullOrEmpty(tipoIndicador) ? "" : $" AND ti.cod_tpindicador = '{tipoIndicador}'"},
            {"filtroFragViolao", fragViolao.HasValue ? $" AND ur.flg_violacaomensal = {(fragViolao.Value ? "1" : "0")}" : ""},
            {"filtroIsContestacao", (isContestacao.HasValue && isContestacao.Value) ? " AND c.id_contestacao IS NOT NULL" : ""}
        };

        var sqlQuery = GetQuery("QUERY_BUSCAR_VIEW_SSCl_POR_FILTRO", parametros);

        return await _context.Set<ConsultaIndicadorSSCLView>()
            .FromSqlRaw(sqlQuery)
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }



    public async Task<IEnumerable<SupervisaoTempoRealView>> SupervisaoTempoResultadoIndicador(string codAgente, string data, CancellationToken cancellationToken)
    {
        var parametros = new Dictionary<string, string>
    {
        {"codAgente", codAgente},
        {"dataFormatada", data.Replace('/', '-')}
    };

        var sqlQuery = GetQuery("QUERY_SUPERVISAO_TEMPO_REAL", parametros);

        return await _context.Set<SupervisaoTempoRealView>()
            .FromSqlRaw(sqlQuery)
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }


    public async Task<IEnumerable<ScadaView>> FiltrarAgentesScadaAsync(string anoMes, string codAgente, CancellationToken cancellationToken)
    {
        var parametros = new Dictionary<string, string>
        {
            {"anoMesFormatado", anoMes.Replace('/', '-')},
            {"codAgente", codAgente}
        };

        var sqlQuery = GetQuery("QUERY_FILTRAR_AGENTES_SCADA", parametros);

        return await _context.Set<ScadaView>()
            .FromSqlRaw(sqlQuery)
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }


    public string BuscarDataReferenciaMaisRecente()
    {
        return _context.ResultadoIndicador.Max(x => x.AnoMesReferencia);
    }

    public IEnumerable<ConsultaIndicadorSSCLView> BuscarViewPorFiltro(string datasString, List<string> ageMrid, string tipoIndicador, bool? fragViolao, bool? isContestacao)
    {
        var parametros = new Dictionary<string, string>
        {
            {"agentes", string.Join("','", ageMrid)},
            {"datasString", datasString},
            {"tipoIndicadorCondition", !string.IsNullOrEmpty(tipoIndicador) ? $"AND ti.cod_tpindicador = '{tipoIndicador}'" : ""},
            {"fragViolaoCondition", fragViolao.HasValue ? $"AND ur.flg_violacaomensal = {(fragViolao.Value ? "1" : "0")}" : ""},
            {"isContestacaoCondition", isContestacao.HasValue && isContestacao.Value ? "AND c.id_contestacao IS NOT NULL" : ""}
        };

        var sqlQuery = GetQuery("QUERY_CONSULTA_INDICADOR_SSCL", parametros);

        return _context.Set<ConsultaIndicadorSSCLView>()
            .FromSqlRaw(sqlQuery)
            .AsNoTracking()
            .ToArray();
    }



    public async Task<IEnumerable<SgiView>> BuscarSgiViewAsync(int idResultadoIndicador, CancellationToken cancellationToken)
    {
        var query = new StringBuilder($@"
        select distinct s.*  from tb_resultadoindicador ri
        inner join tb_resultadoindicador_sgi ris on ris.id_resultadoindicador=ri.id_resultadoindicador
        inner join tb_sgi s on s.num_ons=ris.num_ons and s.num_sequenciasgi=ris.num_sequenciasgi
        where ri.id_resultadoindicador = {idResultadoIndicador} ");

        var scadaList = await _context.Set<SgiView>()
            .FromSqlRaw(query.ToString())
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        return scadaList;
    }

}
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using ONS.PortalMQDI.Data.Context;
using Microsoft.EntityFrameworkCore;
using ONS.PortalMQDI.Data.Entity.View;

namespace ONS.PortalMQDI.Data.Repositories.Single
{
    public class CargaRepository
    {
        #region PortalMQDIDbContext
        protected readonly PortalMQDIDbContext _context;

        public CargaRepository(PortalMQDIDbContext context)
        {
            _context = context;
        }

        #endregion


        public List<ConsultaIndicadorAgenteView> BuscarAgente(string mesAno, string mrid)
        {
            var sqlQuery = new StringBuilder(@"
                select tr.nom_tprecurso, a.nom_curto, a.nom_longo, a.ido_ons, ri.*, ti.nom_tpindicador, ti.cod_tpindicador, c.dsc_contestacao, rc.cmn_resultado, c.id_contestacao,
                rc.id_tpresultadocontestacao
                from tb_agente a 
                inner join tb_resultadoindicador ri on a.age_mrid=ri.mrid and a.anomes_referencia=ri.anomes_referencia
                inner join tb_tprecurso tr on tr.id_tprecurso=ri.id_tprecurso
                inner join tb_tpindicador ti on ti.id_tpindicador = ri.id_tpindicador
                left join tb_contestacao c on c.id_resultadoindicador=ri.id_resultadoindicador
                left join tb_resultadocontestacao rc on rc.id_contestacao = c.id_contestacao
                where ri.anomes_referencia = '");

            sqlQuery.Append(mesAno);
            sqlQuery.Append("' and a.age_mrid in ('");
            sqlQuery.Append(mrid);
            sqlQuery.Append("')");
            sqlQuery.Append(" order by tr.cod_tprecurso, a.nom_curto, a.anomes_referencia DESC");


            return _context.Set<ConsultaIndicadorAgenteView>()
                .FromSqlRaw(sqlQuery.ToString())
                .AsNoTracking()
                .ToList();
        }

        public List<InstalacaoRecursoRelatorioAgenteView> BuscarInstalacaoRecurso(string mesAno, string mrid)
        {
            #region Query
            var sqlQuery = @$"
                        select
                            ri.id_resultadoindicador as ri_id_resultadoindicador, i.nom_curto, i.nom_longo, i.ins_mrid,
                            ri.*, ti.cod_tpindicador as ti_cod_tpindicador, tr.nom_tprecurso,
                            g.dsc_grandeza, g.cod_lscinf as lscinf,g.ido_ons as grandeza,g.tprede, g.nom_enderecofisico
                        from tb_resultadoindicador ri
                        inner join tb_tpindicador ti on ti.id_tpindicador=ri.id_tpindicador
                        inner join tb_tprecurso tr on tr.id_tprecurso=ri.id_tprecurso
                        inner join tb_grandeza g on g.grd_mrid=ri.mrid and g.anomes_referencia=ri.anomes_referencia
                        inner join tb_instalacao i on i.ins_mrid=g.ins_mrid and i.anomes_referencia=ri.anomes_referencia
                        where ri.age_mrid=@mrid and ri.anomes_referencia=@anoMes
                        order by ri.id_tpindicador, i.nom_curto";
            #endregion

            List<InstalacaoRecursoRelatorioAgenteView> resultado = _context.Set<InstalacaoRecursoRelatorioAgenteView>()
                     .FromSqlRaw(sqlQuery, 
                        new SqlParameter("@mrid", mrid),
                        new SqlParameter("@anoMes", mesAno))
                     .AsNoTracking()
                     .ToList();

            return resultado;
        }

        public IndicadorInstalacaoRelatorioAgenteView ObterIndicadorInstalacao(string mesAno, string mrid, string insMrid, int idTipoIndicador)
        {
            #region Query
            var sqlQuery = @$"
                        select
                            ri.id_resultadoindicador as ri_id_resultadoindicador, i.nom_curto, i.nom_longo, i.ins_mrid,
                            ri.*, ti.cod_tpindicador as ti_cod_tpindicador, tr.nom_tprecurso
                        from tb_resultadoindicador ri
                        inner join tb_tpindicador ti on ti.id_tpindicador=ri.id_tpindicador
                        inner join tb_tprecurso tr on tr.id_tprecurso=ri.id_tprecurso
                        inner join tb_instalacao i on i.ins_mrid=ri.mrid and i.anomes_referencia=ri.anomes_referencia
                        where ri.age_mrid=@mrid and ri.anomes_referencia=@anoMes and i.ins_mrid=@insMrid and ri.id_tpindicador=@idTipoIndicador";
            #endregion

            IndicadorInstalacaoRelatorioAgenteView resultado = _context.Set<IndicadorInstalacaoRelatorioAgenteView>()
                     .FromSqlRaw(sqlQuery,
                        new SqlParameter("@mrid", mrid),
                        new SqlParameter("@anoMes", mesAno),
                        new SqlParameter("@insMrid", insMrid),
                        new SqlParameter("@idTipoIndicador", idTipoIndicador))
                     .FirstOrDefault();

            return resultado;
        }

        public List<ConsultaIndicadorSSCLView> BuscarSSCL(string mesAno, string mrid)
        {

            var sqlQuery = new StringBuilder(@"
                Select DISTINCT u.ido_ons as UTR_CD, u.cod_lscinf, ur.*,  ti.cod_tpindicador, c.dsc_contestacao, rc.cmn_resultado, c.id_contestacao, rc.id_tpresultadocontestacao
                From  tb_resultadoindicador ur
                INNER JOIN  tb_utrcd u ON u.utrcd_mrid=ur.mrid and u.anomes_referencia=ur.anomes_referencia
                INNER JOIN  tb_tpindicador ti on ti.id_tpindicador = ur.id_tpindicador
                LEFT JOIN tb_contestacao c on c.id_resultadoindicador=ur.id_resultadoindicador
                LEFT JOIN tb_resultadocontestacao rc on rc.id_contestacao = c.id_contestacao
                INNER JOIN
                (select distinct  g.cod_lscinf, g.anomes_referencia,g.age_mrid from tb_grandeza g
                where g.age_mrid in ('");

            sqlQuery.Append(mrid);
            sqlQuery.Append("') and g.cod_lscinf is not null and g.anomes_referencia in('");
            sqlQuery.Append(mesAno);
            sqlQuery.Append("')) as gr on gr.cod_lscinf=u.cod_lscinf and gr.anomes_referencia=u.anomes_referencia");
            sqlQuery.Append(" order by ur.anomes_referencia, UTR_CD");


            return _context.Set<ConsultaIndicadorSSCLView>()
                .FromSqlRaw(sqlQuery.ToString())
                .AsNoTracking()
                .ToList();
        }

        public bool VerificarDataExiste(string data)
        {
            var query = $"SELECT DISTINCT anomes_referencia FROM tb_resultadoindicador where anomes_referencia='{data}'";

            return _context.Set<CalendarioDataExisteView>()
                .FromSqlRaw(query.ToString())
                .AsNoTracking()
                .Count() > 0;

        }

    }
}

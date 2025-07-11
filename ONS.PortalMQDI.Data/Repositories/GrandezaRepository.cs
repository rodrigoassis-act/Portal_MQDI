using goobeeteams.Entity.Repositories;
using Microsoft.EntityFrameworkCore;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Data.Repositories
{
    public class GrandezaRepository : RepositoryAsync<Grandeza>, IGrandezaRepository
    {
        public GrandezaRepository(PortalMQDIDbContext context) : base(context)
        { }


        public IEnumerable<DadosInstalacaoView> BuscarInstalacaoRangerData(List<DateTime> rangeDatas)
        {
            var datas = rangeDatas.Select(data => data.ToString("yyyy-MM")).ToList();
            var parametros = new Dictionary<string, string>
            {
                {"datasString", string.Join("','", datas)}
            };

            var sqlQuery = GetQuery("QUERY_BUSCAR_INSTALACAO_RANGE_DATA", parametros);

            return _context.Set<DadosInstalacaoView>()
                .FromSqlRaw(sqlQuery)
                .AsNoTracking()
                .ToArray();
        }



        public async Task<IEnumerable<InstalacaoConsultarMedidaView>> InstalacaoConsultarMedidaAsync(string ageMrid, string anoMes, CancellationToken cancellationToke)
        {
            var parametros = new Dictionary<string, string>
            {
                {"anoMes", anoMes},
                {"ageMrid", ageMrid}
            };

            var sqlQuery = GetQuery("QUERY_INSTALACAO_CONSULTAR_MEDIDA", parametros);

            return await _context.Set<InstalacaoConsultarMedidaView>()
                .FromSqlRaw(sqlQuery)
                .AsNoTracking()
                .ToArrayAsync(cancellationToke);
        }



        public IEnumerable<InstalacaoConsultarMedidaView> InstalacaoConsultarMedida(List<string> ageMrid, string anoMes)
        {
            var parametros = new Dictionary<string, string>
            {
                {"anoMes", anoMes},
                {"ageMridList", string.Join(",", ageMrid.Select(x => $"'{x}'"))}
            };

            var sqlQuery = GetQuery("QUERY_INSTALACAO_CONSULTAR_MEDIDA_LIST", parametros);

            return _context.Set<InstalacaoConsultarMedidaView>()
                .FromSqlRaw(sqlQuery)
                .AsNoTracking()
                .ToArray();
        }
    }
}
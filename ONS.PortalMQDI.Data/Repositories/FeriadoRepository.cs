using Microsoft.EntityFrameworkCore;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ONS.PortalMQDI.Data.Repositories
{
    public class FeriadoRepository : RepositoryAsync<Feriado>, IFeriadoRepository
    {
        private readonly PortalMQDIDbContext _context;
        public FeriadoRepository(PortalMQDIDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Feriado> ListaFeriadosNoMes(string anoMesReferencia)
        {
            var mesAno = anoMesReferencia.Split('-');
            DateTime dataReferencia = new DateTime(int.Parse(mesAno[0]), int.Parse(mesAno[1]), 1, 0, 0, 0);
            DateTime dataFinalJanela = dataReferencia.AddMonths(2);

            return _context.Feriado
                .AsNoTracking()
                .Where(r =>
                ((r.DataFeriado.Value.Month < dataFinalJanela.Month) || (r.DataFeriado.Value.Month == dataFinalJanela.Month && r.DataFeriado.Value.Day <= dataFinalJanela.Day))
                &&
                ((r.DataFeriado.Value.Month > dataReferencia.Month) || (r.DataFeriado.Value.Month == dataReferencia.Month) && r.DataFeriado.Value.Day >= dataReferencia.Day));
        }
    }
}

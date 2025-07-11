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
    public class CalendarioSistemaRepository : RepositoryAsync<CalendarioSistema>, ICalendarioSistemaRepository
    {
        private readonly PortalMQDIDbContext _context;
        public CalendarioSistemaRepository(PortalMQDIDbContext context) : base(context)
        {
            _context = context;
        }

        public bool AdicionarRegistro(CalendarioSistema calenadrio)
        {
            _context.CalendariosSistema.Add(calenadrio);
            return _context.SaveChanges() > 0 ? true : false;
        }

        public async Task<CalendarioSistema> BuscaDatasPorDataReferenciaAsync(string date, CancellationToken cancellationToken)
        {
            return await _context.CalendariosSistema.FirstOrDefaultAsync(x => x.DataReferenciaIndicador == date.Replace("/", "-"), cancellationToken);
        }

        public IEnumerable<CalendarioSistema> PegarTodosCalendario()
        {
            return _context.CalendariosSistema.AsNoTracking().OrderByDescending(c => c.DataReferenciaIndicador).ToArray();
        }

        public async Task<IEnumerable<string>> VerificarLocalidadeExistente(string data, CancellationToken cancellationToken)
        {
            return await _context.ResultadoIndicador
                .AsNoTracking()
                .Where(x => x.AnoMesReferencia == data)
                .Select(x => x.CosId)
                .Distinct()
                .ToArrayAsync(cancellationToken);
        }
    }
}
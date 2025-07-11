using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Data.Interfaces
{
    public interface IRelatorioRepository : IRepositoryAsync<Relatorio>
    {
        Task<IEnumerable<string>> BuscarDatasDownloadAsync(CancellationToken cancellationToke);
        Task<IEnumerable<Relatorio>> BuscarAgenteAsync(Expression<Func<Relatorio, bool>> predicate, CancellationToken cancellationToken);
        Task<bool> DeletarPorFiltroAsync(Expression<Func<Relatorio, bool>> predicate, CancellationToken cancellationToken);
        Task<IEnumerable<Relatorio>> BuscarTodosRelatorioAsync(CancellationToken cancellationToken);
        bool ConsultarCentroProcessamento(string centro, string anoMes);
        IEnumerable<Relatorio> BuscarTodosMigrationAsync();
    }
}

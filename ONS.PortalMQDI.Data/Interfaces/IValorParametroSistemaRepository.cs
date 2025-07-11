using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Data.Interfaces
{
    public interface IValorParametroSistemaRepository : IRepositoryAsync<ValorParametroSistema>
    {
        Task<IEnumerable<ValorParametroSistema>> BuscarValorLimiteAsync(CancellationToken cancellationToke);
        Task<ValorParametroSistema> BuscarParametroTerminoPeriodoAnaliseContestacaoAsync(CancellationToken cancellationToke);
        Task<IEnumerable<ValorParametroSistema>> BuscaValorParametroSistemaAsync(CancellationToken cancellationToke);
        Task<ValorParametroSistema> RetornaValorDeParametroAtualPorIdParametroAsync(int id, CancellationToken cancellationToken);
        ValorParametroSistema RetornaValorAtualDoParametroPorNome(string nomeParametro);
        Task<int> ExecutarQueryLimpezaCompletaAsync(string query, CancellationToken cancellationToken);
        IEnumerable<ValorParametroSistema> RetornaParametrosBy(Expression<Func<ValorParametroSistema, bool>> predicate);
    }
}
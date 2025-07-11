using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Data.Repository
{
    public interface IRepositoryAsync<TEntity> : IDisposable where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken);
        Task<IEnumerable<TEntity>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<bool> InsertAsync(TEntity entity, CancellationToken cancellationToken);
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
        Task UpdateAsync(object id, TEntity entity, CancellationToken cancellationToken);
        Task<int> CountAsync(CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task ExecuteTransactionAsync(Func<Task> action, CancellationToken cancellationToken);
        Task<IEnumerable<TEntity>> FromSqlRaw(string sql, params object[] parameters);
        Task<TEntity> FirstItemAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        string GetQuery(string key, Dictionary<string, string> parameters = null);

    }
}

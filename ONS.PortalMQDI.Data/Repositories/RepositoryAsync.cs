using Microsoft.EntityFrameworkCore;
using ONS.PortalMQDI.Data.Context;
using ONS.PortalMQDI.Data.Repository;
using ONS.PortalMQDI.Data.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace goobeeteams.Entity.Repositories
{
    public class RepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class
    {
        protected readonly PortalMQDIDbContext _context;
        protected readonly DbSet<TEntity> _entities;

        public RepositoryAsync(PortalMQDIDbContext context)
        {
            _context = context;
            _entities = context.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _entities.AsNoTracking().ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _entities.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
        }

        public virtual async Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken)
        {
            return await _entities.FindAsync(new object[] { id }, cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await _entities.AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        }

        public virtual async Task<bool> InsertAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _entities.AddAsync(entity, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken) > 0 ? true : false;
        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _entities.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateAsync(object id, TEntity entity, CancellationToken cancellationToken)
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return await _entities.CountAsync(cancellationToken);
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _entities.AnyAsync(predicate, cancellationToken);
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task ExecuteTransactionAsync(Func<Task> action, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await action();
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> FromSqlRaw(string sql, params object[] parameters)
        {
            return await _entities.FromSqlRaw(sql, parameters).ToListAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<TEntity> FirstItemAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _entities.AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public string GetQuery(string key, Dictionary<string, string> parameters = null)
        {
            string query = DatabaseQueries.ResourceManager.GetString(key) ?? string.Empty;

            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentException($"Não foi encontrada uma consulta associada à chave '{key}'.");
            }

            StringBuilder queryBuilder = new StringBuilder(query);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    string placeholder = "{placeholder}";
                    queryBuilder.Replace(placeholder.Replace("placeholder", param.Key), param.Value ?? string.Empty);
                }
            }

            return queryBuilder.ToString();
        }
    }
}

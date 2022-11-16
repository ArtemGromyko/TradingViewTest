using System.Linq.Expressions;

namespace TradingView.DAL.Abstractions.Repositories;

public interface IRepositoryBase<TEntity>
{
    Task AddCollectionAsync(IEnumerable<TEntity> collection, CancellationToken ct = default);
    Task AddAsync(TEntity entity, CancellationToken ct = default);
    Task<List<TEntity>> GetAllAsync(CancellationToken ct = default);
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default);
    Task<List<TEntity>> GetCollectionAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default);
    Task DeleteAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default);
    Task DeleteAllAsync(CancellationToken ct = default);
}

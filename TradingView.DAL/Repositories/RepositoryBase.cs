using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;
using TradingView.DAL.Abstractions.Repositories;
using TradingView.DAL.Settings;

namespace TradingView.DAL.Repositories;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
{
    protected readonly IMongoCollection<TEntity> _collection;
    private string _collectionName;

    public RepositoryBase(IOptions<DatabaseSettings> settings, string collectionName)
    {
        var mongoClient = new MongoClient(
            settings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            settings.Value.DatabaseName);

        _collection = mongoDatabase.GetCollection<TEntity>(collectionName);
        _collectionName = collectionName;
    }

    public async Task AddCollectionAsync(IEnumerable<TEntity> collection, CancellationToken ct = default) =>
        await _collection.InsertManyAsync(collection, cancellationToken: ct);

    public async Task AddAsync(TEntity entity, CancellationToken ct = default) =>
        await _collection.InsertOneAsync(entity, cancellationToken: ct);

    public async Task<List<TEntity>> GetAllAsync(CancellationToken ct = default) =>
        await _collection.AsQueryable().ToListAsync(ct);

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default) =>
        await _collection.AsQueryable().FirstOrDefaultAsync(expression, ct);

    public async Task<List<TEntity>> GetCollectionAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default) =>
        await _collection.Find(expression).ToListAsync(ct);

    public async Task DeleteAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default) =>
       await _collection.DeleteManyAsync(expression, ct);

    public async Task DeleteAllAsync(CancellationToken ct = default)
    {
        await _collection.Database.DropCollectionAsync(_collectionName);
    }
}
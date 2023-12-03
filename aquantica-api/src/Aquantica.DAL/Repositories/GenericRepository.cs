using System.Linq.Expressions;
using Aquantica.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aquantica.DAL.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _dbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericRepository{TEntity}"/> class.
    /// </summary>
    /// <param name="appContext">PlayPrismContext database context.</param>
    public GenericRepository(ApplicationDbContext appContext)
    {
        _dbSet = appContext.Set<TEntity>();
    }

    /// <inheritdoc />
    public IQueryable<TEntity> GetAll()
    {
        var entities = _dbSet.AsQueryable();
        return entities;
    }
    
    /// <inheritdoc />
    public IQueryable<TEntity> GetAllByCondition(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = _dbSet.AsQueryable().Where(predicate);
        return entities;
    }

    /// <inheritdoc />
    public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }
    
    

    /// <inheritdoc />
    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <inheritdoc />
    public async Task<IList<TEntity>> GetByConditionAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TEntity>> selector = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet;

        if (selector != null)
        {
            query = query
                .Where(predicate)
                .Select(selector);
        }
        else
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync(cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        CancellationToken cancellationToken = default)
    {
        if (predicate is not null)
        {
            return await _dbSet.AnyAsync(predicate, cancellationToken);

        }

        return await _dbSet.AnyAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(TEntity obj, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(obj, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddRangeAsync(IEnumerable<TEntity> obj, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(obj, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public void Update(TEntity obj)
    {
        _dbSet.Entry(obj).State = EntityState.Modified;
        //_dbSet.Update(obj);
    }

    /// <inheritdoc />
    public void Delete(TEntity obj)
    {
        try
        {
            _dbSet.Remove(obj);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<int> DeleteAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var entities = _dbSet.Where(predicate);
        var deletedRows = await entities.ExecuteDeleteAsync(cancellationToken);

        return deletedRows;
    }
}

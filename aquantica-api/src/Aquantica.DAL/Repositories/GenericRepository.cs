using System.Linq.Expressions;
using Aquantica.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aquantica.DAL.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly ApplicationDbContext _appContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericRepository{TEntity}"/> class.
    /// </summary>
    /// <param name="appContext">PlayPrismContext database context.</param>
    public GenericRepository(ApplicationDbContext appContext)
    {
        _appContext = appContext;
    }

    /// <inheritdoc />
    public IQueryable<TEntity> GetAll()
    {
        var entities = _appContext.Set<TEntity>().AsQueryable();
        return entities;
    }
    
    /// <inheritdoc />
    public IQueryable<TEntity> GetAllByCondition(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = _appContext.Set<TEntity>().AsQueryable().Where(predicate);
        return entities;
    }

    /// <inheritdoc />
    public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _appContext.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
    }
    
    

    /// <inheritdoc />
    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await _appContext.Set<TEntity>().FindAsync(id);
    }

    /// <inheritdoc />
    public async Task<IList<TEntity>> GetByConditionAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TEntity>> selector = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _appContext.Set<TEntity>().AsQueryable();

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
            return await _appContext.Set<TEntity>().AnyAsync(predicate, cancellationToken);

        }

        return await _appContext.Set<TEntity>().AnyAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(TEntity obj, CancellationToken cancellationToken = default)
    {
        await _appContext.Set<TEntity>().AddAsync(obj, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddRangeAsync(IEnumerable<TEntity> obj, CancellationToken cancellationToken = default)
    {
        await _appContext.Set<TEntity>().AddRangeAsync(obj, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public void Update(TEntity obj)
    {
        _appContext.Set<TEntity>().Entry(obj).State = EntityState.Modified;
        //_dbSet.Update(obj);
    }

    /// <inheritdoc />
    public void Delete(TEntity obj)
    {
        try
        {
            _appContext.Set<TEntity>().Remove(obj);
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
        var entities = _appContext.Set<TEntity>().Where(predicate);
        var deletedRows = await entities.ExecuteDeleteAsync(cancellationToken);

        return deletedRows;
    }
}

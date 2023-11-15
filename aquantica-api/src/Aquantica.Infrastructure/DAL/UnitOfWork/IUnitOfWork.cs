using Microsoft.EntityFrameworkCore.Storage;

namespace Aquantica.Infrastructure.DAL.UnitOfWork;

/// <summary>
/// Interface for UnitOfWork design pattern that works with business transactions.
/// </summary>
public interface IUnitOfWork
{
    Task SaveAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task<IDbContextTransaction> CreateTransactionAsync();


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task CommitTransactionAsync();


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task RollbackTransactionAsync();
}

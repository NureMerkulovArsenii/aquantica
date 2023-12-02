using Aquantica.Core.Entities;
using Aquantica.DAL.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Aquantica.DAL.UnitOfWork;

/// <summary>
/// Interface for UnitOfWork design pattern that works with business transactions.
/// </summary>
public interface IUnitOfWork
{
    IGenericRepository<User> UserRepository { get; }

    IGenericRepository<Role> RoleRepository { get; }

    IGenericRepository<AccessAction> AccessActionRepository { get; }

    IGenericRepository<RefreshToken> RefreshTokenRepository { get; }
    
    IGenericRepository<Settings> SettingsRepository { get; }

    Task<IDbContextTransaction> CreateTransactionAsync();

    Task SaveAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();
}

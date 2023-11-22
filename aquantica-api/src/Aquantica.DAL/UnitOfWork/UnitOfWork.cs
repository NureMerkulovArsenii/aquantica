using Aquantica.Core.Entities;
using Aquantica.DAL.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Aquantica.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    private readonly Lazy<IGenericRepository<User>> _accountRepository;
    private readonly Lazy<IGenericRepository<Role>> _roleRepository;
    private readonly Lazy<IGenericRepository<AccessAction>> _accessActionRepository;
    private readonly Lazy<IGenericRepository<RefreshToken>> _refreshTokenRepository;

    public UnitOfWork(
        ApplicationDbContext context,
        Lazy<IGenericRepository<User>> accountRepository,
        Lazy<IGenericRepository<Role>> roleRepository,
        Lazy<IGenericRepository<AccessAction>> accessActionRepository,
        Lazy<IGenericRepository<RefreshToken>> refreshTokenRepository
        )
    {
        _context = context;
        _accountRepository = accountRepository;
        _roleRepository = roleRepository;
        _accessActionRepository = accessActionRepository;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public IGenericRepository<User> AccountRepository => _accountRepository.Value;

    public IGenericRepository<Role> RoleRepository => _roleRepository.Value;

    public IGenericRepository<AccessAction> AccessActionRepository => _accessActionRepository.Value;

    public IGenericRepository<RefreshToken> RefreshTokenRepository => _refreshTokenRepository.Value;

    public Task<IDbContextTransaction> CreateTransactionAsync()
    {
        return _context.Database.BeginTransactionAsync();
    }

    public Task CommitTransactionAsync()
    {
        return _context.Database.CommitTransactionAsync();
    }


    public Task RollbackTransactionAsync()
    {
        if (_context.Database.CurrentTransaction != null)
            return _context.Database.RollbackTransactionAsync();

        return Task.CompletedTask;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();

        _context.Database.CurrentTransaction?.Dispose();
    }
}

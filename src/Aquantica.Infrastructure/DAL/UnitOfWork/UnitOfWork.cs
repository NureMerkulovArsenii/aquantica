using Microsoft.EntityFrameworkCore.Storage;

namespace Aquantica.Infrastructure.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task CommitTransactionAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IDbContextTransaction> CreateTransactionAsync()
    {
        throw new NotImplementedException();
    }

    public Task RollbackTransactionAsync()
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync()
    {
        throw new NotImplementedException();
    }
}

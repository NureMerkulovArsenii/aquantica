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
    
    IGenericRepository<IrrigationEvent> IrrigationHistoryRepository { get; }
    
    IGenericRepository<IrrigationSection> SectionRepository { get; }
    
    IGenericRepository<Location> LocationRepository { get; }
    
    IGenericRepository<IrrigationSectionType> SectionTypeRepository { get; }
    
    IGenericRepository<IrrigationRuleset> RulesetRepository { get; }
    
    IGenericRepository<WeatherForecast> WeatherForecastRepository { get; }
    
    IGenericRepository<WeatherRecord> WeatherRecordRepository { get; }
    
    IGenericRepository<BackgroundJob> BackgroundJobRepository { get; }
    
    IGenericRepository<BackgroundJobEvent> BackgroundJobEventRepository { get; }
    
    IGenericRepository<MenuItem> MenuItemRepository { get; }

    Task<IDbContextTransaction> CreateTransactionAsync();
    
    IDbContextTransaction CreateTransaction();

    Task SaveAsync();
    
    void Save();

    Task CommitTransactionAsync();
    
    void CommitTransaction();

    Task RollbackTransactionAsync();
    
    void RollbackTransaction();
}

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
    private readonly Lazy<IGenericRepository<Setting>> _settingsRepository;
    private readonly Lazy<IGenericRepository<IrrigationEvent>> _irrigationHistoryRepository;
    private readonly Lazy<IGenericRepository<IrrigationSection>> _sectionRepository;
    private readonly Lazy<IGenericRepository<Location>> _locationRepository;
    private readonly Lazy<IGenericRepository<IrrigationSectionType>> _sectionTypeRepository;
    private readonly Lazy<IGenericRepository<IrrigationRuleset>> _rulesetRepository;
    private readonly Lazy<IGenericRepository<WeatherForecast>> _weatherForecastsRepository;
    private readonly Lazy<IGenericRepository<WeatherRecord>> _weatherRecordsRepository;
    private readonly Lazy<IGenericRepository<BackgroundJob>> _backGroundJobRepository;
    private readonly Lazy<IGenericRepository<BackgroundJobEvent>> _backGroundJobEventRepository;
    private readonly Lazy<IGenericRepository<MenuItem>> _menuItemRepository;
    private readonly Lazy<IGenericRepository<SensorData>> _sensorDataRepository;

    public UnitOfWork(
        ApplicationDbContext context,
        Lazy<IGenericRepository<User>> accountRepository,
        Lazy<IGenericRepository<Role>> roleRepository,
        Lazy<IGenericRepository<AccessAction>> accessActionRepository,
        Lazy<IGenericRepository<RefreshToken>> refreshTokenRepository,
        Lazy<IGenericRepository<Setting>> settingsRepository,
        Lazy<IGenericRepository<IrrigationEvent>> irrigationHistoryRepository,
        Lazy<IGenericRepository<IrrigationSection>> sectionRepository,
        Lazy<IGenericRepository<Location>> locationRepository,
        Lazy<IGenericRepository<IrrigationSectionType>> sectionTypeRepository,
        Lazy<IGenericRepository<IrrigationRuleset>> rulesetRepository,
        Lazy<IGenericRepository<WeatherForecast>> weatherForecastsRepository,
        Lazy<IGenericRepository<WeatherRecord>> weatherRecordsRepository,
        Lazy<IGenericRepository<BackgroundJob>> backGroundJobRepository,
        Lazy<IGenericRepository<BackgroundJobEvent>> backGroundJobEventRepository,
        Lazy<IGenericRepository<MenuItem>> menuItemRepository,
        Lazy<IGenericRepository<SensorData>> sensorDataRepository)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _accountRepository = accountRepository;
        _roleRepository = roleRepository;
        _accessActionRepository = accessActionRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _settingsRepository = settingsRepository;
        _irrigationHistoryRepository = irrigationHistoryRepository;
        _sectionRepository = sectionRepository;
        _locationRepository = locationRepository;
        _sectionTypeRepository = sectionTypeRepository;
        _rulesetRepository = rulesetRepository;
        _weatherForecastsRepository = weatherForecastsRepository;
        _weatherRecordsRepository = weatherRecordsRepository;
        _backGroundJobRepository = backGroundJobRepository;
        _backGroundJobEventRepository = backGroundJobEventRepository;
        _menuItemRepository = menuItemRepository;
        _sensorDataRepository = sensorDataRepository;
    }

    public IGenericRepository<User> UserRepository => _accountRepository.Value;

    public IGenericRepository<Role> RoleRepository => _roleRepository.Value;

    public IGenericRepository<AccessAction> AccessActionRepository => _accessActionRepository.Value;

    public IGenericRepository<RefreshToken> RefreshTokenRepository => _refreshTokenRepository.Value;

    public IGenericRepository<Setting> SettingsRepository => _settingsRepository.Value;

    public IGenericRepository<IrrigationEvent> IrrigationEventRepository => _irrigationHistoryRepository.Value;

    public IGenericRepository<IrrigationSection> SectionRepository => _sectionRepository.Value;
    
    public IGenericRepository<Location> LocationRepository => _locationRepository.Value;

    public IGenericRepository<IrrigationSectionType> SectionTypeRepository => _sectionTypeRepository.Value;

    public IGenericRepository<IrrigationRuleset> RulesetRepository => _rulesetRepository.Value;
    
    public IGenericRepository<WeatherForecast> WeatherForecastRepository => _weatherForecastsRepository.Value;
    
    public IGenericRepository<WeatherRecord> WeatherRecordRepository => _weatherRecordsRepository.Value;
    
    public IGenericRepository<BackgroundJob> BackgroundJobRepository => _backGroundJobRepository.Value;
    
    public IGenericRepository<BackgroundJobEvent> BackgroundJobEventRepository => _backGroundJobEventRepository.Value;
    
    public IGenericRepository<MenuItem> MenuItemRepository => _menuItemRepository.Value;
    
    public IGenericRepository<SensorData> SensorDataRepository => _sensorDataRepository.Value;

    public Task<IDbContextTransaction> CreateTransactionAsync()
    {
        return _context.Database.BeginTransactionAsync();
    }
    
    public IDbContextTransaction CreateTransaction()
    {
        return _context.Database.BeginTransaction();
    }

    public Task CommitTransactionAsync()
    {
        return _context.Database.CommitTransactionAsync();
    }
    
    public void CommitTransaction()
    {
        _context.Database.CommitTransaction();
    }

    public Task RollbackTransactionAsync()
    {
        if (_context.Database.CurrentTransaction != null)
            return _context.Database.RollbackTransactionAsync();

        return Task.CompletedTask;
    }
    
    public void RollbackTransaction()
    {
        if (_context.Database.CurrentTransaction != null)
            _context.Database.RollbackTransaction();
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();

        _context.Database.CurrentTransaction?.Dispose();
    }
    
    public void Save()
    {
        _context.SaveChanges();

        _context.Database.CurrentTransaction?.Dispose();
    }
}